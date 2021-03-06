﻿using System;
using Core;
using Core.Domain;

namespace Infrastructure.Services.Discount {
	public class LoyaltyProgrammService : ILoyaltyProgrammService {
		public LoyaltyProgrammInfo GetInfo(string cardNumber) {
			if (cardNumber == "22222")
				throw new LoyaltyServiceException("Can not process loyalty card: " + cardNumber);

			return new LoyaltyProgrammInfo(LoyaltyCardType.Gold, new DateTime(2015, 01, 01));
		}
	}
}