﻿using Microsoft.AspNetCore.Mvc;
using MonolithicSampleRestApi.Domain.Extensions;
using MonolithicSampleRestApi.Domain.Models.ApiModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MonolithicSampleRestApi.Controllers.V1
{
    [ApiController]
    public class FinancialController : ApiBaseController
    {
        [HttpGet]
        [Route("/financial/v1/interestRate")]
        public async Task<decimal> GetInterestRate()
        {
            return 0.01m;
        }

        [HttpPost]
        [Route("/financial/v1/calcInterest")]
        public async Task<ActionResult<CalcInterest>> PostCalcInterest(RequestCalcInterest request)
        {
            decimal interestRate = await GetInterestRate();

            if (request.InitialValue <= 0)
                ModelState.AddModelError("InitialValue", "Deve ser maior que 0");
            if (request.MonthQuantity <= 0)
                ModelState.AddModelError("MonthQuantity", "Deve ser maior que 0");

            if (ModelState.IsValid)
            {
                double dInitialValue = request.InitialValue.ToDouble();
                double pow1 = 1d + (interestRate.ToDouble());
                double pow2 = request.MonthQuantity.ToDouble();
                double dInterestRate = Math.Pow(pow1, pow2);
                double r = dInitialValue * dInterestRate;
                decimal finalValue = decimal.Round(r.ToDecimal(), 2, MidpointRounding.ToZero);

                return Ok(new CalcInterest(finalValue));
            }

            return BadRequest(ModelState);
        }
    }
}
