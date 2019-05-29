using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Emceelee.OverflowCalculator
{
    public static class ProgressiveRounder
    {
        public static List<decimal> GetSplits(decimal amount, int participants, int decimals = 2)
        {
            int decimalFactor = (int) Math.Pow(10, decimals);

            decimal truncatedAmount = Math.Floor(amount * decimalFactor) / decimalFactor;
            decimal participantShare = Math.Floor(truncatedAmount * decimalFactor / participants) / decimalFactor;  //to X decimal places
            bool isFinalLevel = participantShare == 0;

            var splits = new List<decimal>();
            var remainder = truncatedAmount;

            if(isFinalLevel)
            {
                participantShare = 1M / decimalFactor;

                for (int i = 0; i < participants; ++i)
                {
                    if(remainder > 0)
                    {
                        splits.Add(participantShare);
                        remainder -= participantShare;
                    }
                    else
                    {
                        splits.Add(0);
                    }
                }
            }
            else
            {
                for (int i = 0; i < participants; ++i)
                {
                    splits.Add(participantShare);
                    remainder -= participantShare;
                }

                var remainderSplits = GetSplits(remainder, participants, decimals);
                splits = MemberwiseSum(splits, remainderSplits);
            }

            return splits;
        }

        private static List<decimal> MemberwiseSum(List<decimal> list1, List<decimal> list2)
        {
            Debug.Assert(list1.Count == list2.Count);

            var result = new List<decimal>();

            for(int i = 0; i < list1.Count; ++i)
            {
                result.Add(list1[i] + list2[i]);
            }

            return result;
        }
    }
}
