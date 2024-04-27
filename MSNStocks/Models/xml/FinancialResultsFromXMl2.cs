using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSNStocks.Models.xml
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class InBseFinAdditionalTier1Ratio
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("@unitRef")]
        public string unitRef { get; set; }

        [JsonProperty("@decimals")]
        public string decimals { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinAdjustmentsForDecreaseIncreaseInInventories
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("@unitRef")]
        public string unitRef { get; set; }

        [JsonProperty("@decimals")]
        public string decimals { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinAdjustmentsForDecreaseIncreaseInOtherCurrentAssets
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("@unitRef")]
        public string unitRef { get; set; }

        [JsonProperty("@decimals")]
        public string decimals { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinAdjustmentsForDecreaseIncreaseInTradeReceivables
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("@unitRef")]
        public string unitRef { get; set; }

        [JsonProperty("@decimals")]
        public string decimals { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinAdjustmentsForDepreciationAndAmortisationExpense
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("@unitRef")]
        public string unitRef { get; set; }

        [JsonProperty("@decimals")]
        public string decimals { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinAdjustmentsForDividendIncome
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("@unitRef")]
        public string unitRef { get; set; }

        [JsonProperty("@decimals")]
        public string decimals { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinAdjustmentsForFinanceCosts
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("@unitRef")]
        public string unitRef { get; set; }

        [JsonProperty("@decimals")]
        public string decimals { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinAdjustmentsForImpairmentLossReversalOfImpairmentLossRecognisedInProfitOrLoss
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("@unitRef")]
        public string unitRef { get; set; }

        [JsonProperty("@decimals")]
        public string decimals { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinAdjustmentsForIncreaseDecreaseInOtherCurrentLiabilities
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("@unitRef")]
        public string unitRef { get; set; }

        [JsonProperty("@decimals")]
        public string decimals { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinAdjustmentsForIncreaseDecreaseInTradePayables
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("@unitRef")]
        public string unitRef { get; set; }

        [JsonProperty("@decimals")]
        public string decimals { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinAdjustmentsForProvisions
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("@unitRef")]
        public string unitRef { get; set; }

        [JsonProperty("@decimals")]
        public string decimals { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinAdjustmentsForReconcileProfitLoss
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("@unitRef")]
        public string unitRef { get; set; }

        [JsonProperty("@decimals")]
        public string decimals { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinAdjustmentsForSharebasedPayments
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("@unitRef")]
        public string unitRef { get; set; }

        [JsonProperty("@decimals")]
        public string decimals { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinAdjustmentsForUnrealisedForeignExchangeLossesGains
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("@unitRef")]
        public string unitRef { get; set; }

        [JsonProperty("@decimals")]
        public string decimals { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinAdjustmentsForWorkingCapital
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("@unitRef")]
        public string unitRef { get; set; }

        [JsonProperty("@decimals")]
        public string decimals { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinAdjustmentsToProfitLoss
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("@unitRef")]
        public string unitRef { get; set; }

        [JsonProperty("@decimals")]
        public string decimals { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinAdvances
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("@unitRef")]
        public string unitRef { get; set; }

        [JsonProperty("@decimals")]
        public string decimals { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinAssets
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinBalancesWithBanksAndMoneyAtCallAndShortNotice
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("@unitRef")]
        public string unitRef { get; set; }

        [JsonProperty("@decimals")]
        public string decimals { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinBasicEarningsPerShareAfterExtraordinaryItem
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("@unitRef")]
        public string unitRef { get; set; }

        [JsonProperty("@decimals")]
        public string decimals { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinBasicEarningsPerShareBeforeExtraordinaryItem
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("@unitRef")]
        public string unitRef { get; set; }

        [JsonProperty("@decimals")]
        public string decimals { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinBorrowings
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("@unitRef")]
        public string unitRef { get; set; }

        [JsonProperty("@decimals")]
        public string decimals { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinCapital
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("@unitRef")]
        public string unitRef { get; set; }

        [JsonProperty("@decimals")]
        public string decimals { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinCapitalAndLiabilities
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("@unitRef")]
        public string unitRef { get; set; }

        [JsonProperty("@decimals")]
        public string decimals { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinCashAdvancesAndLoansMadeToOtherPartiesClassifiedAsInvestingActivities
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("@unitRef")]
        public string unitRef { get; set; }

        [JsonProperty("@decimals")]
        public string decimals { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinCashAndBalancesWithReserveBankOfIndia
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("@unitRef")]
        public string unitRef { get; set; }

        [JsonProperty("@decimals")]
        public string decimals { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinCashAndCashEquivalentsCashFlowStatement
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("@unitRef")]
        public string unitRef { get; set; }

        [JsonProperty("@decimals")]
        public string decimals { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinCashFlowsFromLosingControlOfSubsidiariesOrOtherBusinessesClassifiedAsInvestingActivities
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("@unitRef")]
        public string unitRef { get; set; }

        [JsonProperty("@decimals")]
        public string decimals { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinCashFlowsFromUsedInFinancingActivities
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("@unitRef")]
        public string unitRef { get; set; }

        [JsonProperty("@decimals")]
        public string decimals { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinCashFlowsFromUsedInFinancingActivitiesBeforeExtraordinaryItems
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("@unitRef")]
        public string unitRef { get; set; }

        [JsonProperty("@decimals")]
        public string decimals { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinCashFlowsFromUsedInInvestingActivities
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("@unitRef")]
        public string unitRef { get; set; }

        [JsonProperty("@decimals")]
        public string decimals { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinCashFlowsFromUsedInInvestingActivitiesBeforeExtraordinaryItems
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("@unitRef")]
        public string unitRef { get; set; }

        [JsonProperty("@decimals")]
        public string decimals { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinCashFlowsFromUsedInOperatingActivities
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("@unitRef")]
        public string unitRef { get; set; }

        [JsonProperty("@decimals")]
        public string decimals { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinCashFlowsFromUsedInOperatingActivitiesBeforeExtraordinaryItems
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("@unitRef")]
        public string unitRef { get; set; }

        [JsonProperty("@decimals")]
        public string decimals { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinCashFlowsFromUsedInOperations
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("@unitRef")]
        public string unitRef { get; set; }

        [JsonProperty("@decimals")]
        public string decimals { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinCashFlowsUsedInObtainingControlOfSubsidiariesOrOtherBusinessesClassifiedAsInvestingActivities
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("@unitRef")]
        public string unitRef { get; set; }

        [JsonProperty("@decimals")]
        public string decimals { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinCashPaymentForInvestmentInPartnershipFirmOrAssociationOfPersonsOrLimitedLiabilityPartnerships
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("@unitRef")]
        public string unitRef { get; set; }

        [JsonProperty("@decimals")]
        public string decimals { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinCashPaymentsForFutureContractsForwardContractsOptionContractsAndSwapContractsClassifiedAsInvestingActivities
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("@unitRef")]
        public string unitRef { get; set; }

        [JsonProperty("@decimals")]
        public string decimals { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinCashReceiptsFromFutureContractsForwardContractsOptionContractsAndSwapContractsClassifiedAsInvestingActivities
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("@unitRef")]
        public string unitRef { get; set; }

        [JsonProperty("@decimals")]
        public string decimals { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinCashReceiptsFromRepaymentOfAdvancesAndLoansMadeToOtherPartiesClassifiedAsInvestingActivities
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("@unitRef")]
        public string unitRef { get; set; }

        [JsonProperty("@decimals")]
        public string decimals { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinCashReceiptsFromShareOfProfitsOfPartnershipFirmOrAssociationOfPersonsOrLimitedLiabilityPartnerships
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("@unitRef")]
        public string unitRef { get; set; }

        [JsonProperty("@decimals")]
        public string decimals { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinCET1Ratio
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("@unitRef")]
        public string unitRef { get; set; }

        [JsonProperty("@decimals")]
        public string decimals { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinClassOfSecurity
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinDateOfBoardMeetingWhenFinancialResultsWereApproved
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinDateOfEndOfBoardMeeting
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinDateOfEndOfFinancialYear
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinDateOfEndOfReportingPeriod
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinDateOfStartOfBoardMeeting
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinDateOfStartOfFinancialYear
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinDateOfStartOfReportingPeriod
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinDateOnWhichPriorIntimationOfTheMeetingForConsideringFinancialResultsWasInformedToTheExchange
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinDeclarationOfUnmodifiedOpinionOrStatementOnImpactOfAuditQualification
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinDeposits
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("@unitRef")]
        public string unitRef { get; set; }

        [JsonProperty("@decimals")]
        public string decimals { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinDescriptionOfOperatingExpense
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("@unitRef")]
        public string unitRef { get; set; }

        [JsonProperty("@decimals")]
        public string decimals { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinDescriptionOfPresentationCurrency
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinDescriptionOfReportableSegment
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("@unitRef")]
        public string unitRef { get; set; }

        [JsonProperty("@decimals")]
        public string decimals { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinDilutedEarningsPerShareAfterExtraordinaryItem
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("@unitRef")]
        public string unitRef { get; set; }

        [JsonProperty("@decimals")]
        public string decimals { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinDilutedEarningsPerShareBeforeExtraordinaryItem
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("@unitRef")]
        public string unitRef { get; set; }

        [JsonProperty("@decimals")]
        public string decimals { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinDisclosureOfNotesOnAssetsAndLiabilitiesExplanatoryTextBlock
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinDisclosureOfNotesOnFinancialResultsExplanatoryTextBlock
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinDividendsPaidClassifiedAsFinancingActivities
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("@unitRef")]
        public string unitRef { get; set; }

        [JsonProperty("@decimals")]
        public string decimals { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinDividendsReceivedClassifiedAsInvestingActivities
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("@unitRef")]
        public string unitRef { get; set; }

        [JsonProperty("@decimals")]
        public string decimals { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinDividendsReceivedClassifiedAsOperatingActivities
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("@unitRef")]
        public string unitRef { get; set; }

        [JsonProperty("@decimals")]
        public string decimals { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinEffectOfExchangeRateChangesOnCashAndCashEquivalents
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("@unitRef")]
        public string unitRef { get; set; }

        [JsonProperty("@decimals")]
        public string decimals { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinEmployeesCost
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("@unitRef")]
        public string unitRef { get; set; }

        [JsonProperty("@decimals")]
        public string decimals { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinEndTimeOfBoardMeeting
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinExceptionalItem
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("@unitRef")]
        public string unitRef { get; set; }

        [JsonProperty("@decimals")]
        public string decimals { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinExpenditureExcludingProvisionsAndContingency
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("@unitRef")]
        public string unitRef { get; set; }

        [JsonProperty("@decimals")]
        public string decimals { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinExtraordinaryItem
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("@unitRef")]
        public string unitRef { get; set; }

        [JsonProperty("@decimals")]
        public string decimals { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinFaceValueOfEquityShareCapital
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("@unitRef")]
        public string unitRef { get; set; }

        [JsonProperty("@decimals")]
        public string decimals { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinFixedAssets
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("@unitRef")]
        public string unitRef { get; set; }

        [JsonProperty("@decimals")]
        public string decimals { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinGrossNonPerformingAsset
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("@unitRef")]
        public string unitRef { get; set; }

        [JsonProperty("@decimals")]
        public string decimals { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinIncome
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("@unitRef")]
        public string unitRef { get; set; }

        [JsonProperty("@decimals")]
        public string decimals { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinIncomeTaxesPaidRefundClassifiedAsFinancingActivities
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("@unitRef")]
        public string unitRef { get; set; }

        [JsonProperty("@decimals")]
        public string decimals { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinIncomeTaxesPaidRefundClassifiedAsInvestingActivities
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("@unitRef")]
        public string unitRef { get; set; }

        [JsonProperty("@decimals")]
        public string decimals { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinIncomeTaxesPaidRefundClassifiedAsOperatingActivities
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("@unitRef")]
        public string unitRef { get; set; }

        [JsonProperty("@decimals")]
        public string decimals { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinIncreaseDecreaseInCashAndCashEquivalents
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("@unitRef")]
        public string unitRef { get; set; }

        [JsonProperty("@decimals")]
        public string decimals { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinIncreaseDecreaseInCashAndCashEquivalentsBeforeEffectOfExchangeRateChanges
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("@unitRef")]
        public string unitRef { get; set; }

        [JsonProperty("@decimals")]
        public string decimals { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinInterestEarned
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("@unitRef")]
        public string unitRef { get; set; }

        [JsonProperty("@decimals")]
        public string decimals { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinInterestExpended
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("@unitRef")]
        public string unitRef { get; set; }

        [JsonProperty("@decimals")]
        public string decimals { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinInterestOnBalancesWithReserveBankOfIndiaAndOtherInterBankFund
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("@unitRef")]
        public string unitRef { get; set; }

        [JsonProperty("@decimals")]
        public string decimals { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinInterestOrDiscountOnAdvancesOrBill
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("@unitRef")]
        public string unitRef { get; set; }

        [JsonProperty("@decimals")]
        public string decimals { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinInterestPaidClassifiedAsFinancingActivities
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("@unitRef")]
        public string unitRef { get; set; }

        [JsonProperty("@decimals")]
        public string decimals { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinInterestPaidClassifiedAsOperatingActivities
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("@unitRef")]
        public string unitRef { get; set; }

        [JsonProperty("@decimals")]
        public string decimals { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinInterestReceivedClassifiedAsInvestingActivities
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("@unitRef")]
        public string unitRef { get; set; }

        [JsonProperty("@decimals")]
        public string decimals { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinInterestReceivedClassifiedAsOperatingActivities
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("@unitRef")]
        public string unitRef { get; set; }

        [JsonProperty("@decimals")]
        public string decimals { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinInterSegmentRevenue
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("@unitRef")]
        public string unitRef { get; set; }

        [JsonProperty("@decimals")]
        public string decimals { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinInvestments
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("@unitRef")]
        public string unitRef { get; set; }

        [JsonProperty("@decimals")]
        public string decimals { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinIsCompanyReportingMultisegmentOrSingleSegment
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinISIN
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinLevelOfRoundingUsedInFinancialStatements
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinMSEISymbol
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinNameOfBank
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinNatureOfReportStandaloneConsolidated
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinNetSegmentAssets
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinNetSegmentLiabilities
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinNonPerformingAsset
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("@unitRef")]
        public string unitRef { get; set; }

        [JsonProperty("@decimals")]
        public string decimals { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinOperatingExpense
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("@unitRef")]
        public string unitRef { get; set; }

        [JsonProperty("@decimals")]
        public string decimals { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinOperatingProfitBeforeProvisionAndContingency
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("@unitRef")]
        public string unitRef { get; set; }

        [JsonProperty("@decimals")]
        public string decimals { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinOtherAdjustmentsForNoncashItems
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("@unitRef")]
        public string unitRef { get; set; }

        [JsonProperty("@decimals")]
        public string decimals { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinOtherAdjustmentsForWhichCashEffectsAreInvestingOrFinancingCashFlow
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("@unitRef")]
        public string unitRef { get; set; }

        [JsonProperty("@decimals")]
        public string decimals { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinOtherAdjustmentsToReconcileProfitLoss
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("@unitRef")]
        public string unitRef { get; set; }

        [JsonProperty("@decimals")]
        public string decimals { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinOtherAssets
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("@unitRef")]
        public string unitRef { get; set; }

        [JsonProperty("@decimals")]
        public string decimals { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinOtherCashPaymentsToAcquireEquityOrDebtInstrumentsOfOtherEntitiesClassifiedAsInvestingActivities
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("@unitRef")]
        public string unitRef { get; set; }

        [JsonProperty("@decimals")]
        public string decimals { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinOtherCashPaymentsToAcquireInterestsInJointVenturesClassifiedAsInvestingActivities
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("@unitRef")]
        public string unitRef { get; set; }

        [JsonProperty("@decimals")]
        public string decimals { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinOtherCashReceiptsFromSalesOfEquityOrDebtInstrumentsOfOtherEntitiesClassifiedAsInvestingActivities
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("@unitRef")]
        public string unitRef { get; set; }

        [JsonProperty("@decimals")]
        public string decimals { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinOtherCashReceiptsFromSalesOfInterestsInJointVenturesClassifiedAsInvestingActivities
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("@unitRef")]
        public string unitRef { get; set; }

        [JsonProperty("@decimals")]
        public string decimals { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinOtherIncome
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("@unitRef")]
        public string unitRef { get; set; }

        [JsonProperty("@decimals")]
        public string decimals { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinOtherInflowsOutflowsOfCashClassifiedAsFinancingActivities
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("@unitRef")]
        public string unitRef { get; set; }

        [JsonProperty("@decimals")]
        public string decimals { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinOtherInflowsOutflowsOfCashClassifiedAsInvestingActivities
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("@unitRef")]
        public string unitRef { get; set; }

        [JsonProperty("@decimals")]
        public string decimals { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinOtherInflowsOutflowsOfCashClassifiedAsOperatingActivities
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("@unitRef")]
        public string unitRef { get; set; }

        [JsonProperty("@decimals")]
        public string decimals { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinOtherInterest
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("@unitRef")]
        public string unitRef { get; set; }

        [JsonProperty("@decimals")]
        public string decimals { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinOtherLiabilitiesAndProvisions
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("@unitRef")]
        public string unitRef { get; set; }

        [JsonProperty("@decimals")]
        public string decimals { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinOtherOperatingExpense
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("@unitRef")]
        public string unitRef { get; set; }

        [JsonProperty("@decimals")]
        public string decimals { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinOtherUnallocableExpenditureNetOffUnAllocableIncome
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("@unitRef")]
        public string unitRef { get; set; }

        [JsonProperty("@decimals")]
        public string decimals { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinPaidUpValueOfEquityShareCapital
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("@unitRef")]
        public string unitRef { get; set; }

        [JsonProperty("@decimals")]
        public string decimals { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinPaymentForExtraordinaryItemsClassifiedAsFinancingActivities
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("@unitRef")]
        public string unitRef { get; set; }

        [JsonProperty("@decimals")]
        public string decimals { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinPaymentForExtraordinaryItemsClassifiedAsInvestingActivities
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("@unitRef")]
        public string unitRef { get; set; }

        [JsonProperty("@decimals")]
        public string decimals { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinPaymentForExtraordinaryItemsClassifiedAsOperatingActivities
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("@unitRef")]
        public string unitRef { get; set; }

        [JsonProperty("@decimals")]
        public string decimals { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinPercentageOfGrossNpa
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("@unitRef")]
        public string unitRef { get; set; }

        [JsonProperty("@decimals")]
        public string decimals { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinPercentageOfNpa
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinPercentageOfShareHeldByGovernmentOfIndium
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("@unitRef")]
        public string unitRef { get; set; }

        [JsonProperty("@decimals")]
        public string decimals { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinProceedsFromBorrowingsClassifiedAsFinancingActivities
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("@unitRef")]
        public string unitRef { get; set; }

        [JsonProperty("@decimals")]
        public string decimals { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinProceedsFromExtraordinaryItemsClassifiedAsFinancingActivities
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("@unitRef")]
        public string unitRef { get; set; }

        [JsonProperty("@decimals")]
        public string decimals { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinProceedsFromExtraordinaryItemsClassifiedAsInvestingActivities
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("@unitRef")]
        public string unitRef { get; set; }

        [JsonProperty("@decimals")]
        public string decimals { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinProceedsFromExtraordinaryItemsClassifiedAsOperatingActivities
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("@unitRef")]
        public string unitRef { get; set; }

        [JsonProperty("@decimals")]
        public string decimals { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinProceedsFromGovernmentGrantsClassifiedAsInvestingActivities
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("@unitRef")]
        public string unitRef { get; set; }

        [JsonProperty("@decimals")]
        public string decimals { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinProceedsFromIssuingDebenturesNotesBondsEtc
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("@unitRef")]
        public string unitRef { get; set; }

        [JsonProperty("@decimals")]
        public string decimals { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinProceedsFromIssuingOtherEquityInstruments
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("@unitRef")]
        public string unitRef { get; set; }

        [JsonProperty("@decimals")]
        public string decimals { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinProceedsFromIssuingShares
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("@unitRef")]
        public string unitRef { get; set; }

        [JsonProperty("@decimals")]
        public string decimals { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinProceedsFromSalesOfIntangibleAssetsClassifiedAsInvestingActivities
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("@unitRef")]
        public string unitRef { get; set; }

        [JsonProperty("@decimals")]
        public string decimals { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinProceedsFromSalesOfTangibleAssetsClassifiedAsInvestingActivities
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("@unitRef")]
        public string unitRef { get; set; }

        [JsonProperty("@decimals")]
        public string decimals { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinProfitLossAfterTaxesMinorityInterestAndShareOfProfitLossOfAssociate
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("@unitRef")]
        public string unitRef { get; set; }

        [JsonProperty("@decimals")]
        public string decimals { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinProfitLossForThePeriod
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("@unitRef")]
        public string unitRef { get; set; }

        [JsonProperty("@decimals")]
        public string decimals { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinProfitLossFromOrdinaryActivitiesAfterTax
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("@unitRef")]
        public string unitRef { get; set; }

        [JsonProperty("@decimals")]
        public string decimals { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinProfitLossFromOrdinaryActivitiesBeforeTax
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("@unitRef")]
        public string unitRef { get; set; }

        [JsonProperty("@decimals")]
        public string decimals { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinProfitLossOfMinorityInterest
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("@unitRef")]
        public string unitRef { get; set; }

        [JsonProperty("@decimals")]
        public string decimals { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinProvisionsOtherThanTaxAndContingency
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("@unitRef")]
        public string unitRef { get; set; }

        [JsonProperty("@decimals")]
        public string decimals { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinPurchaseOfIntangibleAssetsClassifiedAsInvestingActivities
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("@unitRef")]
        public string unitRef { get; set; }

        [JsonProperty("@decimals")]
        public string decimals { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinPurchaseOfTangibleAssetsClassifiedAsInvestingActivities
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("@unitRef")]
        public string unitRef { get; set; }

        [JsonProperty("@decimals")]
        public string decimals { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinRepaymentsOfBorrowingsClassifiedAsFinancingActivities
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("@unitRef")]
        public string unitRef { get; set; }

        [JsonProperty("@decimals")]
        public string decimals { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinReportingQuarter
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinReserveExcludingRevaluationReserves
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("@unitRef")]
        public string unitRef { get; set; }

        [JsonProperty("@decimals")]
        public string decimals { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinReservesAndSurplus
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("@unitRef")]
        public string unitRef { get; set; }

        [JsonProperty("@decimals")]
        public string decimals { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinResultType
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinReturnOnAsset
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("@unitRef")]
        public string unitRef { get; set; }

        [JsonProperty("@decimals")]
        public string decimals { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinRevenueOnInvestment
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("@unitRef")]
        public string unitRef { get; set; }

        [JsonProperty("@decimals")]
        public string decimals { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinScripCode
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinSegmentAsset
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("@unitRef")]
        public string unitRef { get; set; }

        [JsonProperty("@decimals")]
        public string decimals { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinSegmentFinanceCost
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("@unitRef")]
        public string unitRef { get; set; }

        [JsonProperty("@decimals")]
        public string decimals { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinSegmentLiability
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("@unitRef")]
        public string unitRef { get; set; }

        [JsonProperty("@decimals")]
        public string decimals { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinSegmentProfitBeforeTax
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinSegmentProfitLossBeforeTaxAndFinanceCost
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("@unitRef")]
        public string unitRef { get; set; }

        [JsonProperty("@decimals")]
        public string decimals { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinSegmentRevenue
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("@unitRef")]
        public string unitRef { get; set; }

        [JsonProperty("@decimals")]
        public string decimals { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinSegmentRevenueFromOperation
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinShareOfProfitAndLossFromPartnershipFirmOrAssociationOfPersonsOrLimitedLiabilityPartnerships
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("@unitRef")]
        public string unitRef { get; set; }

        [JsonProperty("@decimals")]
        public string decimals { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinShareOfProfitLossOfAssociate
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("@unitRef")]
        public string unitRef { get; set; }

        [JsonProperty("@decimals")]
        public string decimals { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinStartTimeOfBoardMeeting
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinSymbol
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinTaxExpense
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("@unitRef")]
        public string unitRef { get; set; }

        [JsonProperty("@decimals")]
        public string decimals { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinTypeOfCashFlowStatement
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinUnAllocableAsset
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("@unitRef")]
        public string unitRef { get; set; }

        [JsonProperty("@decimals")]
        public string decimals { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinUnAllocableLiability
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("@unitRef")]
        public string unitRef { get; set; }

        [JsonProperty("@decimals")]
        public string decimals { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinWhetherCashFlowStatementIsApplicableOnCompany
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinWhetherResultsAreAuditedOrUnaudited
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class LinkSchemaRef
    {
        [JsonProperty("@xlink:type")]
        public string xlinktype { get; set; }

        [JsonProperty("@xlink:href")]
        public string xlinkhref { get; set; }
    }

    public class FinancialResultsFromXMl2
    {
        [JsonProperty("?xml")]
        public Xml xml { get; set; }

        [JsonProperty("xbrli:xbrl")]
        public XbrliXbrl xbrlixbrl { get; set; }
    }

    public class XbrldiExplicitMember
    {
        [JsonProperty("@dimension")]
        public string dimension { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class XbrliContext
    {
        [JsonProperty("@id")]
        public string id { get; set; }

        [JsonProperty("xbrli:entity")]
        public XbrliEntity xbrlientity { get; set; }

        [JsonProperty("xbrli:period")]
        public XbrliPeriod xbrliperiod { get; set; }

        [JsonProperty("xbrli:scenario")]
        public XbrliScenario xbrliscenario { get; set; }
    }

    public class XbrliDivide
    {
        [JsonProperty("xbrli:unitNumerator")]
        public XbrliUnitNumerator xbrliunitNumerator { get; set; }

        [JsonProperty("xbrli:unitDenominator")]
        public XbrliUnitDenominator xbrliunitDenominator { get; set; }
    }

    public class XbrliEntity
    {
        [JsonProperty("xbrli:identifier")]
        public XbrliIdentifier xbrliidentifier { get; set; }
    }

    public class XbrliIdentifier
    {
        [JsonProperty("@scheme")]
        public string scheme { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class XbrliPeriod
    {
        [JsonProperty("xbrli:startDate")]
        public string xbrlistartDate { get; set; }

        [JsonProperty("xbrli:endDate")]
        public string xbrliendDate { get; set; }

        [JsonProperty("xbrli:instant")]
        public string xbrliinstant { get; set; }
    }

    public class XbrliScenario
    {
        [JsonProperty("xbrldi:explicitMember")]
        public XbrldiExplicitMember xbrldiexplicitMember { get; set; }
    }

    public class XbrliUnit
    {
        [JsonProperty("@id")]
        public string id { get; set; }

        [JsonProperty("xbrli:measure")]
        public string xbrlimeasure { get; set; }

        [JsonProperty("xbrli:divide")]
        public XbrliDivide xbrlidivide { get; set; }
    }

    public class XbrliUnitDenominator
    {
        [JsonProperty("xbrli:measure")]
        public string xbrlimeasure { get; set; }
    }

    public class XbrliUnitNumerator
    {
        [JsonProperty("xbrli:measure")]
        public string xbrlimeasure { get; set; }
    }

    public class XbrliXbrl
    {
        [JsonProperty("@xmlns:in-bse-fin")]
        public string xmlnsinbsefin { get; set; }

        [JsonProperty("@xmlns:in-bse-fin-roles")]
        public string xmlnsinbsefinroles { get; set; }

        [JsonProperty("@xmlns:in-bse-fin-ent")]
        public string xmlnsinbsefinent { get; set; }

        [JsonProperty("@xmlns:in-bse-fin-type")]
        public string xmlnsinbsefintype { get; set; }

        [JsonProperty("@xmlns:xbrldt")]
        public string xmlnsxbrldt { get; set; }

        [JsonProperty("@xmlns:nonnum")]
        public string xmlnsnonnum { get; set; }

        [JsonProperty("@xmlns:link")]
        public string xmlnslink { get; set; }

        [JsonProperty("@xmlns:net")]
        public string xmlnsnet { get; set; }

        [JsonProperty("@xmlns:num")]
        public string xmlnsnum { get; set; }

        [JsonProperty("@xmlns:xlink")]
        public string xmlnsxlink { get; set; }

        [JsonProperty("@xmlns:iso4217")]
        public string xmlnsiso4217 { get; set; }

        [JsonProperty("@xmlns:negated")]
        public string xmlnsnegated { get; set; }

        [JsonProperty("@xmlns:xbrldi")]
        public string xmlnsxbrldi { get; set; }

        [JsonProperty("@xmlns:xbrli")]
        public string xmlnsxbrli { get; set; }

        [JsonProperty("@xmlns:xl")]
        public string xmlnsxl { get; set; }

        [JsonProperty("link:schemaRef")]
        public LinkSchemaRef linkschemaRef { get; set; }

        [JsonProperty("xbrli:context")]
        public List<XbrliContext> xbrlicontext { get; set; }

        [JsonProperty("xbrli:unit")]
        public List<XbrliUnit> xbrliunit { get; set; }

        [JsonProperty("in-bse-fin:ScripCode")]
        public InBseFinScripCode inbsefinScripCode { get; set; }

        [JsonProperty("in-bse-fin:Symbol")]
        public InBseFinSymbol inbsefinSymbol { get; set; }

        [JsonProperty("in-bse-fin:MSEISymbol")]
        public InBseFinMSEISymbol inbsefinMSEISymbol { get; set; }

        [JsonProperty("in-bse-fin:ISIN")]
        public InBseFinISIN inbsefinISIN { get; set; }

        [JsonProperty("in-bse-fin:NameOfBank")]
        public InBseFinNameOfBank inbsefinNameOfBank { get; set; }

        [JsonProperty("in-bse-fin:ResultType")]
        public InBseFinResultType inbsefinResultType { get; set; }

        [JsonProperty("in-bse-fin:ClassOfSecurity")]
        public InBseFinClassOfSecurity inbsefinClassOfSecurity { get; set; }

        [JsonProperty("in-bse-fin:DateOfStartOfFinancialYear")]
        public InBseFinDateOfStartOfFinancialYear inbsefinDateOfStartOfFinancialYear { get; set; }

        [JsonProperty("in-bse-fin:DateOfEndOfFinancialYear")]
        public InBseFinDateOfEndOfFinancialYear inbsefinDateOfEndOfFinancialYear { get; set; }

        [JsonProperty("in-bse-fin:DateOfBoardMeetingWhenFinancialResultsWereApproved")]
        public InBseFinDateOfBoardMeetingWhenFinancialResultsWereApproved inbsefinDateOfBoardMeetingWhenFinancialResultsWereApproved { get; set; }

        [JsonProperty("in-bse-fin:DateOnWhichPriorIntimationOfTheMeetingForConsideringFinancialResultsWasInformedToTheExchange")]
        public InBseFinDateOnWhichPriorIntimationOfTheMeetingForConsideringFinancialResultsWasInformedToTheExchange inbsefinDateOnWhichPriorIntimationOfTheMeetingForConsideringFinancialResultsWasInformedToTheExchange { get; set; }

        [JsonProperty("in-bse-fin:DescriptionOfPresentationCurrency")]
        public InBseFinDescriptionOfPresentationCurrency inbsefinDescriptionOfPresentationCurrency { get; set; }

        [JsonProperty("in-bse-fin:LevelOfRoundingUsedInFinancialStatements")]
        public InBseFinLevelOfRoundingUsedInFinancialStatements inbsefinLevelOfRoundingUsedInFinancialStatements { get; set; }

        [JsonProperty("in-bse-fin:ReportingQuarter")]
        public InBseFinReportingQuarter inbsefinReportingQuarter { get; set; }

        [JsonProperty("in-bse-fin:IsCompanyReportingMultisegmentOrSingleSegment")]
        public InBseFinIsCompanyReportingMultisegmentOrSingleSegment inbsefinIsCompanyReportingMultisegmentOrSingleSegment { get; set; }

        [JsonProperty("in-bse-fin:StartTimeOfBoardMeeting")]
        public InBseFinStartTimeOfBoardMeeting inbsefinStartTimeOfBoardMeeting { get; set; }

        [JsonProperty("in-bse-fin:EndTimeOfBoardMeeting")]
        public InBseFinEndTimeOfBoardMeeting inbsefinEndTimeOfBoardMeeting { get; set; }

        [JsonProperty("in-bse-fin:DateOfStartOfBoardMeeting")]
        public InBseFinDateOfStartOfBoardMeeting inbsefinDateOfStartOfBoardMeeting { get; set; }

        [JsonProperty("in-bse-fin:DateOfEndOfBoardMeeting")]
        public InBseFinDateOfEndOfBoardMeeting inbsefinDateOfEndOfBoardMeeting { get; set; }

        [JsonProperty("in-bse-fin:WhetherCashFlowStatementIsApplicableOnCompany")]
        public InBseFinWhetherCashFlowStatementIsApplicableOnCompany inbsefinWhetherCashFlowStatementIsApplicableOnCompany { get; set; }

        [JsonProperty("in-bse-fin:TypeOfCashFlowStatement")]
        public InBseFinTypeOfCashFlowStatement inbsefinTypeOfCashFlowStatement { get; set; }

        [JsonProperty("in-bse-fin:DeclarationOfUnmodifiedOpinionOrStatementOnImpactOfAuditQualification")]
        public InBseFinDeclarationOfUnmodifiedOpinionOrStatementOnImpactOfAuditQualification inbsefinDeclarationOfUnmodifiedOpinionOrStatementOnImpactOfAuditQualification { get; set; }

        [JsonProperty("in-bse-fin:DateOfStartOfReportingPeriod")]
        public List<InBseFinDateOfStartOfReportingPeriod> inbsefinDateOfStartOfReportingPeriod { get; set; }

        [JsonProperty("in-bse-fin:DateOfEndOfReportingPeriod")]
        public List<InBseFinDateOfEndOfReportingPeriod> inbsefinDateOfEndOfReportingPeriod { get; set; }

        [JsonProperty("in-bse-fin:WhetherResultsAreAuditedOrUnaudited")]
        public List<InBseFinWhetherResultsAreAuditedOrUnaudited> inbsefinWhetherResultsAreAuditedOrUnaudited { get; set; }

        [JsonProperty("in-bse-fin:NatureOfReportStandaloneConsolidated")]
        public List<InBseFinNatureOfReportStandaloneConsolidated> inbsefinNatureOfReportStandaloneConsolidated { get; set; }

        [JsonProperty("in-bse-fin:InterestOrDiscountOnAdvancesOrBills")]
        public List<InBseFinInterestOrDiscountOnAdvancesOrBill> inbsefinInterestOrDiscountOnAdvancesOrBills { get; set; }

        [JsonProperty("in-bse-fin:RevenueOnInvestments")]
        public List<InBseFinRevenueOnInvestment> inbsefinRevenueOnInvestments { get; set; }

        [JsonProperty("in-bse-fin:InterestOnBalancesWithReserveBankOfIndiaAndOtherInterBankFunds")]
        public List<InBseFinInterestOnBalancesWithReserveBankOfIndiaAndOtherInterBankFund> inbsefinInterestOnBalancesWithReserveBankOfIndiaAndOtherInterBankFunds { get; set; }

        [JsonProperty("in-bse-fin:OtherInterest")]
        public List<InBseFinOtherInterest> inbsefinOtherInterest { get; set; }

        [JsonProperty("in-bse-fin:InterestEarned")]
        public List<InBseFinInterestEarned> inbsefinInterestEarned { get; set; }

        [JsonProperty("in-bse-fin:OtherIncome")]
        public List<InBseFinOtherIncome> inbsefinOtherIncome { get; set; }

        [JsonProperty("in-bse-fin:Income")]
        public List<InBseFinIncome> inbsefinIncome { get; set; }

        [JsonProperty("in-bse-fin:InterestExpended")]
        public List<InBseFinInterestExpended> inbsefinInterestExpended { get; set; }

        [JsonProperty("in-bse-fin:EmployeesCost")]
        public List<InBseFinEmployeesCost> inbsefinEmployeesCost { get; set; }

        [JsonProperty("in-bse-fin:OtherOperatingExpenses")]
        public List<InBseFinOtherOperatingExpense> inbsefinOtherOperatingExpenses { get; set; }

        [JsonProperty("in-bse-fin:OperatingExpenses")]
        public List<InBseFinOperatingExpense> inbsefinOperatingExpenses { get; set; }

        [JsonProperty("in-bse-fin:ExpenditureExcludingProvisionsAndContingencies")]
        public List<InBseFinExpenditureExcludingProvisionsAndContingency> inbsefinExpenditureExcludingProvisionsAndContingencies { get; set; }

        [JsonProperty("in-bse-fin:OperatingProfitBeforeProvisionAndContingencies")]
        public List<InBseFinOperatingProfitBeforeProvisionAndContingency> inbsefinOperatingProfitBeforeProvisionAndContingencies { get; set; }

        [JsonProperty("in-bse-fin:ProvisionsOtherThanTaxAndContingencies")]
        public List<InBseFinProvisionsOtherThanTaxAndContingency> inbsefinProvisionsOtherThanTaxAndContingencies { get; set; }

        [JsonProperty("in-bse-fin:ExceptionalItems")]
        public List<InBseFinExceptionalItem> inbsefinExceptionalItems { get; set; }

        [JsonProperty("in-bse-fin:ProfitLossFromOrdinaryActivitiesBeforeTax")]
        public List<InBseFinProfitLossFromOrdinaryActivitiesBeforeTax> inbsefinProfitLossFromOrdinaryActivitiesBeforeTax { get; set; }

        [JsonProperty("in-bse-fin:TaxExpense")]
        public List<InBseFinTaxExpense> inbsefinTaxExpense { get; set; }

        [JsonProperty("in-bse-fin:ProfitLossFromOrdinaryActivitiesAfterTax")]
        public List<InBseFinProfitLossFromOrdinaryActivitiesAfterTax> inbsefinProfitLossFromOrdinaryActivitiesAfterTax { get; set; }

        [JsonProperty("in-bse-fin:ExtraordinaryItems")]
        public List<InBseFinExtraordinaryItem> inbsefinExtraordinaryItems { get; set; }

        [JsonProperty("in-bse-fin:ProfitLossForThePeriod")]
        public List<InBseFinProfitLossForThePeriod> inbsefinProfitLossForThePeriod { get; set; }

        [JsonProperty("in-bse-fin:ShareOfProfitLossOfAssociates")]
        public List<InBseFinShareOfProfitLossOfAssociate> inbsefinShareOfProfitLossOfAssociates { get; set; }

        [JsonProperty("in-bse-fin:ProfitLossOfMinorityInterest")]
        public List<InBseFinProfitLossOfMinorityInterest> inbsefinProfitLossOfMinorityInterest { get; set; }

        [JsonProperty("in-bse-fin:ProfitLossAfterTaxesMinorityInterestAndShareOfProfitLossOfAssociates")]
        public List<InBseFinProfitLossAfterTaxesMinorityInterestAndShareOfProfitLossOfAssociate> inbsefinProfitLossAfterTaxesMinorityInterestAndShareOfProfitLossOfAssociates { get; set; }

        [JsonProperty("in-bse-fin:PaidUpValueOfEquityShareCapital")]
        public List<InBseFinPaidUpValueOfEquityShareCapital> inbsefinPaidUpValueOfEquityShareCapital { get; set; }

        [JsonProperty("in-bse-fin:FaceValueOfEquityShareCapital")]
        public List<InBseFinFaceValueOfEquityShareCapital> inbsefinFaceValueOfEquityShareCapital { get; set; }

        [JsonProperty("in-bse-fin:PercentageOfShareHeldByGovernmentOfIndia")]
        public List<InBseFinPercentageOfShareHeldByGovernmentOfIndium> inbsefinPercentageOfShareHeldByGovernmentOfIndia { get; set; }

        [JsonProperty("in-bse-fin:CET1Ratio")]
        public List<InBseFinCET1Ratio> inbsefinCET1Ratio { get; set; }

        [JsonProperty("in-bse-fin:AdditionalTier1Ratio")]
        public List<InBseFinAdditionalTier1Ratio> inbsefinAdditionalTier1Ratio { get; set; }

        [JsonProperty("in-bse-fin:BasicEarningsPerShareBeforeExtraordinaryItems")]
        public List<InBseFinBasicEarningsPerShareBeforeExtraordinaryItem> inbsefinBasicEarningsPerShareBeforeExtraordinaryItems { get; set; }

        [JsonProperty("in-bse-fin:DilutedEarningsPerShareBeforeExtraordinaryItems")]
        public List<InBseFinDilutedEarningsPerShareBeforeExtraordinaryItem> inbsefinDilutedEarningsPerShareBeforeExtraordinaryItems { get; set; }

        [JsonProperty("in-bse-fin:BasicEarningsPerShareAfterExtraordinaryItems")]
        public List<InBseFinBasicEarningsPerShareAfterExtraordinaryItem> inbsefinBasicEarningsPerShareAfterExtraordinaryItems { get; set; }

        [JsonProperty("in-bse-fin:DilutedEarningsPerShareAfterExtraordinaryItems")]
        public List<InBseFinDilutedEarningsPerShareAfterExtraordinaryItem> inbsefinDilutedEarningsPerShareAfterExtraordinaryItems { get; set; }

        [JsonProperty("in-bse-fin:GrossNonPerformingAssets")]
        public List<InBseFinGrossNonPerformingAsset> inbsefinGrossNonPerformingAssets { get; set; }

        [JsonProperty("in-bse-fin:NonPerformingAssets")]
        public List<InBseFinNonPerformingAsset> inbsefinNonPerformingAssets { get; set; }

        [JsonProperty("in-bse-fin:PercentageOfGrossNpa")]
        public List<InBseFinPercentageOfGrossNpa> inbsefinPercentageOfGrossNpa { get; set; }

        [JsonProperty("in-bse-fin:PercentageOfNpa")]
        public List<InBseFinPercentageOfNpa> inbsefinPercentageOfNpa { get; set; }

        [JsonProperty("in-bse-fin:ReturnOnAssets")]
        public List<InBseFinReturnOnAsset> inbsefinReturnOnAssets { get; set; }

        [JsonProperty("in-bse-fin:DisclosureOfNotesOnFinancialResultsExplanatoryTextBlock")]
        public InBseFinDisclosureOfNotesOnFinancialResultsExplanatoryTextBlock inbsefinDisclosureOfNotesOnFinancialResultsExplanatoryTextBlock { get; set; }

        [JsonProperty("in-bse-fin:ReserveExcludingRevaluationReserves")]
        public InBseFinReserveExcludingRevaluationReserves inbsefinReserveExcludingRevaluationReserves { get; set; }

        [JsonProperty("in-bse-fin:DescriptionOfOperatingExpenses")]
        public List<InBseFinDescriptionOfOperatingExpense> inbsefinDescriptionOfOperatingExpenses { get; set; }

        [JsonProperty("in-bse-fin:Capital")]
        public InBseFinCapital inbsefinCapital { get; set; }

        [JsonProperty("in-bse-fin:ReservesAndSurplus")]
        public InBseFinReservesAndSurplus inbsefinReservesAndSurplus { get; set; }

        [JsonProperty("in-bse-fin:Deposits")]
        public InBseFinDeposits inbsefinDeposits { get; set; }

        [JsonProperty("in-bse-fin:Borrowings")]
        public InBseFinBorrowings inbsefinBorrowings { get; set; }

        [JsonProperty("in-bse-fin:OtherLiabilitiesAndProvisions")]
        public InBseFinOtherLiabilitiesAndProvisions inbsefinOtherLiabilitiesAndProvisions { get; set; }

        [JsonProperty("in-bse-fin:CapitalAndLiabilities")]
        public InBseFinCapitalAndLiabilities inbsefinCapitalAndLiabilities { get; set; }

        [JsonProperty("in-bse-fin:CashAndBalancesWithReserveBankOfIndia")]
        public InBseFinCashAndBalancesWithReserveBankOfIndia inbsefinCashAndBalancesWithReserveBankOfIndia { get; set; }

        [JsonProperty("in-bse-fin:BalancesWithBanksAndMoneyAtCallAndShortNotice")]
        public InBseFinBalancesWithBanksAndMoneyAtCallAndShortNotice inbsefinBalancesWithBanksAndMoneyAtCallAndShortNotice { get; set; }

        [JsonProperty("in-bse-fin:Investments")]
        public InBseFinInvestments inbsefinInvestments { get; set; }

        [JsonProperty("in-bse-fin:Advances")]
        public InBseFinAdvances inbsefinAdvances { get; set; }

        [JsonProperty("in-bse-fin:FixedAssets")]
        public InBseFinFixedAssets inbsefinFixedAssets { get; set; }

        [JsonProperty("in-bse-fin:OtherAssets")]
        public InBseFinOtherAssets inbsefinOtherAssets { get; set; }

        [JsonProperty("in-bse-fin:Assets")]
        public InBseFinAssets inbsefinAssets { get; set; }

        [JsonProperty("in-bse-fin:DisclosureOfNotesOnAssetsAndLiabilitiesExplanatoryTextBlock")]
        public InBseFinDisclosureOfNotesOnAssetsAndLiabilitiesExplanatoryTextBlock inbsefinDisclosureOfNotesOnAssetsAndLiabilitiesExplanatoryTextBlock { get; set; }

        [JsonProperty("in-bse-fin:DescriptionOfReportableSegment")]
        public List<InBseFinDescriptionOfReportableSegment> inbsefinDescriptionOfReportableSegment { get; set; }

        [JsonProperty("in-bse-fin:SegmentRevenue")]
        public List<InBseFinSegmentRevenue> inbsefinSegmentRevenue { get; set; }

        [JsonProperty("in-bse-fin:InterSegmentRevenue")]
        public List<InBseFinInterSegmentRevenue> inbsefinInterSegmentRevenue { get; set; }

        [JsonProperty("in-bse-fin:SegmentRevenueFromOperations")]
        public List<InBseFinSegmentRevenueFromOperation> inbsefinSegmentRevenueFromOperations { get; set; }

        [JsonProperty("in-bse-fin:SegmentProfitLossBeforeTaxAndFinanceCosts")]
        public List<InBseFinSegmentProfitLossBeforeTaxAndFinanceCost> inbsefinSegmentProfitLossBeforeTaxAndFinanceCosts { get; set; }

        [JsonProperty("in-bse-fin:SegmentFinanceCosts")]
        public List<InBseFinSegmentFinanceCost> inbsefinSegmentFinanceCosts { get; set; }

        [JsonProperty("in-bse-fin:OtherUnallocableExpenditureNetOffUnAllocableIncome")]
        public List<InBseFinOtherUnallocableExpenditureNetOffUnAllocableIncome> inbsefinOtherUnallocableExpenditureNetOffUnAllocableIncome { get; set; }

        [JsonProperty("in-bse-fin:SegmentProfitBeforeTax")]
        public List<InBseFinSegmentProfitBeforeTax> inbsefinSegmentProfitBeforeTax { get; set; }

        [JsonProperty("in-bse-fin:SegmentAssets")]
        public List<InBseFinSegmentAsset> inbsefinSegmentAssets { get; set; }

        [JsonProperty("in-bse-fin:UnAllocableAssets")]
        public List<InBseFinUnAllocableAsset> inbsefinUnAllocableAssets { get; set; }

        [JsonProperty("in-bse-fin:NetSegmentAssets")]
        public InBseFinNetSegmentAssets inbsefinNetSegmentAssets { get; set; }

        [JsonProperty("in-bse-fin:SegmentLiabilities")]
        public List<InBseFinSegmentLiability> inbsefinSegmentLiabilities { get; set; }

        [JsonProperty("in-bse-fin:UnAllocableLiabilities")]
        public List<InBseFinUnAllocableLiability> inbsefinUnAllocableLiabilities { get; set; }

        [JsonProperty("in-bse-fin:NetSegmentLiabilities")]
        public InBseFinNetSegmentLiabilities inbsefinNetSegmentLiabilities { get; set; }

        [JsonProperty("in-bse-fin:AdjustmentsForFinanceCosts")]
        public InBseFinAdjustmentsForFinanceCosts inbsefinAdjustmentsForFinanceCosts { get; set; }

        [JsonProperty("in-bse-fin:AdjustmentsForDepreciationAndAmortisationExpense")]
        public InBseFinAdjustmentsForDepreciationAndAmortisationExpense inbsefinAdjustmentsForDepreciationAndAmortisationExpense { get; set; }

        [JsonProperty("in-bse-fin:AdjustmentsForImpairmentLossReversalOfImpairmentLossRecognisedInProfitOrLoss")]
        public InBseFinAdjustmentsForImpairmentLossReversalOfImpairmentLossRecognisedInProfitOrLoss inbsefinAdjustmentsForImpairmentLossReversalOfImpairmentLossRecognisedInProfitOrLoss { get; set; }

        [JsonProperty("in-bse-fin:AdjustmentsForUnrealisedForeignExchangeLossesGains")]
        public InBseFinAdjustmentsForUnrealisedForeignExchangeLossesGains inbsefinAdjustmentsForUnrealisedForeignExchangeLossesGains { get; set; }

        [JsonProperty("in-bse-fin:AdjustmentsForDividendIncome")]
        public InBseFinAdjustmentsForDividendIncome inbsefinAdjustmentsForDividendIncome { get; set; }

        [JsonProperty("in-bse-fin:AdjustmentsForSharebasedPayments")]
        public InBseFinAdjustmentsForSharebasedPayments inbsefinAdjustmentsForSharebasedPayments { get; set; }

        [JsonProperty("in-bse-fin:OtherAdjustmentsForWhichCashEffectsAreInvestingOrFinancingCashFlow")]
        public InBseFinOtherAdjustmentsForWhichCashEffectsAreInvestingOrFinancingCashFlow inbsefinOtherAdjustmentsForWhichCashEffectsAreInvestingOrFinancingCashFlow { get; set; }

        [JsonProperty("in-bse-fin:OtherAdjustmentsToReconcileProfitLoss")]
        public InBseFinOtherAdjustmentsToReconcileProfitLoss inbsefinOtherAdjustmentsToReconcileProfitLoss { get; set; }

        [JsonProperty("in-bse-fin:OtherAdjustmentsForNoncashItems")]
        public InBseFinOtherAdjustmentsForNoncashItems inbsefinOtherAdjustmentsForNoncashItems { get; set; }

        [JsonProperty("in-bse-fin:ShareOfProfitAndLossFromPartnershipFirmOrAssociationOfPersonsOrLimitedLiabilityPartnerships")]
        public InBseFinShareOfProfitAndLossFromPartnershipFirmOrAssociationOfPersonsOrLimitedLiabilityPartnerships inbsefinShareOfProfitAndLossFromPartnershipFirmOrAssociationOfPersonsOrLimitedLiabilityPartnerships { get; set; }

        [JsonProperty("in-bse-fin:AdjustmentsToProfitLoss")]
        public InBseFinAdjustmentsToProfitLoss inbsefinAdjustmentsToProfitLoss { get; set; }

        [JsonProperty("in-bse-fin:AdjustmentsForDecreaseIncreaseInInventories")]
        public InBseFinAdjustmentsForDecreaseIncreaseInInventories inbsefinAdjustmentsForDecreaseIncreaseInInventories { get; set; }

        [JsonProperty("in-bse-fin:AdjustmentsForDecreaseIncreaseInTradeReceivables")]
        public InBseFinAdjustmentsForDecreaseIncreaseInTradeReceivables inbsefinAdjustmentsForDecreaseIncreaseInTradeReceivables { get; set; }

        [JsonProperty("in-bse-fin:AdjustmentsForDecreaseIncreaseInOtherCurrentAssets")]
        public InBseFinAdjustmentsForDecreaseIncreaseInOtherCurrentAssets inbsefinAdjustmentsForDecreaseIncreaseInOtherCurrentAssets { get; set; }

        [JsonProperty("in-bse-fin:AdjustmentsForIncreaseDecreaseInTradePayables")]
        public InBseFinAdjustmentsForIncreaseDecreaseInTradePayables inbsefinAdjustmentsForIncreaseDecreaseInTradePayables { get; set; }

        [JsonProperty("in-bse-fin:AdjustmentsForIncreaseDecreaseInOtherCurrentLiabilities")]
        public InBseFinAdjustmentsForIncreaseDecreaseInOtherCurrentLiabilities inbsefinAdjustmentsForIncreaseDecreaseInOtherCurrentLiabilities { get; set; }

        [JsonProperty("in-bse-fin:AdjustmentsForProvisions")]
        public InBseFinAdjustmentsForProvisions inbsefinAdjustmentsForProvisions { get; set; }

        [JsonProperty("in-bse-fin:AdjustmentsForWorkingCapital")]
        public InBseFinAdjustmentsForWorkingCapital inbsefinAdjustmentsForWorkingCapital { get; set; }

        [JsonProperty("in-bse-fin:AdjustmentsForReconcileProfitLoss")]
        public InBseFinAdjustmentsForReconcileProfitLoss inbsefinAdjustmentsForReconcileProfitLoss { get; set; }

        [JsonProperty("in-bse-fin:CashFlowsFromUsedInOperations")]
        public InBseFinCashFlowsFromUsedInOperations inbsefinCashFlowsFromUsedInOperations { get; set; }

        [JsonProperty("in-bse-fin:DividendsReceivedClassifiedAsOperatingActivities")]
        public InBseFinDividendsReceivedClassifiedAsOperatingActivities inbsefinDividendsReceivedClassifiedAsOperatingActivities { get; set; }

        [JsonProperty("in-bse-fin:InterestPaidClassifiedAsOperatingActivities")]
        public InBseFinInterestPaidClassifiedAsOperatingActivities inbsefinInterestPaidClassifiedAsOperatingActivities { get; set; }

        [JsonProperty("in-bse-fin:InterestReceivedClassifiedAsOperatingActivities")]
        public InBseFinInterestReceivedClassifiedAsOperatingActivities inbsefinInterestReceivedClassifiedAsOperatingActivities { get; set; }

        [JsonProperty("in-bse-fin:IncomeTaxesPaidRefundClassifiedAsOperatingActivities")]
        public InBseFinIncomeTaxesPaidRefundClassifiedAsOperatingActivities inbsefinIncomeTaxesPaidRefundClassifiedAsOperatingActivities { get; set; }

        [JsonProperty("in-bse-fin:OtherInflowsOutflowsOfCashClassifiedAsOperatingActivities")]
        public InBseFinOtherInflowsOutflowsOfCashClassifiedAsOperatingActivities inbsefinOtherInflowsOutflowsOfCashClassifiedAsOperatingActivities { get; set; }

        [JsonProperty("in-bse-fin:CashFlowsFromUsedInOperatingActivitiesBeforeExtraordinaryItems")]
        public InBseFinCashFlowsFromUsedInOperatingActivitiesBeforeExtraordinaryItems inbsefinCashFlowsFromUsedInOperatingActivitiesBeforeExtraordinaryItems { get; set; }

        [JsonProperty("in-bse-fin:ProceedsFromExtraordinaryItemsClassifiedAsOperatingActivities")]
        public InBseFinProceedsFromExtraordinaryItemsClassifiedAsOperatingActivities inbsefinProceedsFromExtraordinaryItemsClassifiedAsOperatingActivities { get; set; }

        [JsonProperty("in-bse-fin:PaymentForExtraordinaryItemsClassifiedAsOperatingActivities")]
        public InBseFinPaymentForExtraordinaryItemsClassifiedAsOperatingActivities inbsefinPaymentForExtraordinaryItemsClassifiedAsOperatingActivities { get; set; }

        [JsonProperty("in-bse-fin:CashFlowsFromUsedInOperatingActivities")]
        public InBseFinCashFlowsFromUsedInOperatingActivities inbsefinCashFlowsFromUsedInOperatingActivities { get; set; }

        [JsonProperty("in-bse-fin:CashFlowsFromLosingControlOfSubsidiariesOrOtherBusinessesClassifiedAsInvestingActivities")]
        public InBseFinCashFlowsFromLosingControlOfSubsidiariesOrOtherBusinessesClassifiedAsInvestingActivities inbsefinCashFlowsFromLosingControlOfSubsidiariesOrOtherBusinessesClassifiedAsInvestingActivities { get; set; }

        [JsonProperty("in-bse-fin:CashFlowsUsedInObtainingControlOfSubsidiariesOrOtherBusinessesClassifiedAsInvestingActivities")]
        public InBseFinCashFlowsUsedInObtainingControlOfSubsidiariesOrOtherBusinessesClassifiedAsInvestingActivities inbsefinCashFlowsUsedInObtainingControlOfSubsidiariesOrOtherBusinessesClassifiedAsInvestingActivities { get; set; }

        [JsonProperty("in-bse-fin:OtherCashReceiptsFromSalesOfEquityOrDebtInstrumentsOfOtherEntitiesClassifiedAsInvestingActivities")]
        public InBseFinOtherCashReceiptsFromSalesOfEquityOrDebtInstrumentsOfOtherEntitiesClassifiedAsInvestingActivities inbsefinOtherCashReceiptsFromSalesOfEquityOrDebtInstrumentsOfOtherEntitiesClassifiedAsInvestingActivities { get; set; }

        [JsonProperty("in-bse-fin:OtherCashPaymentsToAcquireEquityOrDebtInstrumentsOfOtherEntitiesClassifiedAsInvestingActivities")]
        public InBseFinOtherCashPaymentsToAcquireEquityOrDebtInstrumentsOfOtherEntitiesClassifiedAsInvestingActivities inbsefinOtherCashPaymentsToAcquireEquityOrDebtInstrumentsOfOtherEntitiesClassifiedAsInvestingActivities { get; set; }

        [JsonProperty("in-bse-fin:OtherCashReceiptsFromSalesOfInterestsInJointVenturesClassifiedAsInvestingActivities")]
        public InBseFinOtherCashReceiptsFromSalesOfInterestsInJointVenturesClassifiedAsInvestingActivities inbsefinOtherCashReceiptsFromSalesOfInterestsInJointVenturesClassifiedAsInvestingActivities { get; set; }

        [JsonProperty("in-bse-fin:OtherCashPaymentsToAcquireInterestsInJointVenturesClassifiedAsInvestingActivities")]
        public InBseFinOtherCashPaymentsToAcquireInterestsInJointVenturesClassifiedAsInvestingActivities inbsefinOtherCashPaymentsToAcquireInterestsInJointVenturesClassifiedAsInvestingActivities { get; set; }

        [JsonProperty("in-bse-fin:CashReceiptsFromShareOfProfitsOfPartnershipFirmOrAssociationOfPersonsOrLimitedLiabilityPartnerships")]
        public InBseFinCashReceiptsFromShareOfProfitsOfPartnershipFirmOrAssociationOfPersonsOrLimitedLiabilityPartnerships inbsefinCashReceiptsFromShareOfProfitsOfPartnershipFirmOrAssociationOfPersonsOrLimitedLiabilityPartnerships { get; set; }

        [JsonProperty("in-bse-fin:CashPaymentForInvestmentInPartnershipFirmOrAssociationOfPersonsOrLimitedLiabilityPartnerships")]
        public InBseFinCashPaymentForInvestmentInPartnershipFirmOrAssociationOfPersonsOrLimitedLiabilityPartnerships inbsefinCashPaymentForInvestmentInPartnershipFirmOrAssociationOfPersonsOrLimitedLiabilityPartnerships { get; set; }

        [JsonProperty("in-bse-fin:ProceedsFromSalesOfTangibleAssetsClassifiedAsInvestingActivities")]
        public InBseFinProceedsFromSalesOfTangibleAssetsClassifiedAsInvestingActivities inbsefinProceedsFromSalesOfTangibleAssetsClassifiedAsInvestingActivities { get; set; }

        [JsonProperty("in-bse-fin:PurchaseOfTangibleAssetsClassifiedAsInvestingActivities")]
        public InBseFinPurchaseOfTangibleAssetsClassifiedAsInvestingActivities inbsefinPurchaseOfTangibleAssetsClassifiedAsInvestingActivities { get; set; }

        [JsonProperty("in-bse-fin:ProceedsFromSalesOfIntangibleAssetsClassifiedAsInvestingActivities")]
        public InBseFinProceedsFromSalesOfIntangibleAssetsClassifiedAsInvestingActivities inbsefinProceedsFromSalesOfIntangibleAssetsClassifiedAsInvestingActivities { get; set; }

        [JsonProperty("in-bse-fin:PurchaseOfIntangibleAssetsClassifiedAsInvestingActivities")]
        public InBseFinPurchaseOfIntangibleAssetsClassifiedAsInvestingActivities inbsefinPurchaseOfIntangibleAssetsClassifiedAsInvestingActivities { get; set; }

        [JsonProperty("in-bse-fin:CashAdvancesAndLoansMadeToOtherPartiesClassifiedAsInvestingActivities")]
        public InBseFinCashAdvancesAndLoansMadeToOtherPartiesClassifiedAsInvestingActivities inbsefinCashAdvancesAndLoansMadeToOtherPartiesClassifiedAsInvestingActivities { get; set; }

        [JsonProperty("in-bse-fin:CashReceiptsFromRepaymentOfAdvancesAndLoansMadeToOtherPartiesClassifiedAsInvestingActivities")]
        public InBseFinCashReceiptsFromRepaymentOfAdvancesAndLoansMadeToOtherPartiesClassifiedAsInvestingActivities inbsefinCashReceiptsFromRepaymentOfAdvancesAndLoansMadeToOtherPartiesClassifiedAsInvestingActivities { get; set; }

        [JsonProperty("in-bse-fin:CashPaymentsForFutureContractsForwardContractsOptionContractsAndSwapContractsClassifiedAsInvestingActivities")]
        public InBseFinCashPaymentsForFutureContractsForwardContractsOptionContractsAndSwapContractsClassifiedAsInvestingActivities inbsefinCashPaymentsForFutureContractsForwardContractsOptionContractsAndSwapContractsClassifiedAsInvestingActivities { get; set; }

        [JsonProperty("in-bse-fin:CashReceiptsFromFutureContractsForwardContractsOptionContractsAndSwapContractsClassifiedAsInvestingActivities")]
        public InBseFinCashReceiptsFromFutureContractsForwardContractsOptionContractsAndSwapContractsClassifiedAsInvestingActivities inbsefinCashReceiptsFromFutureContractsForwardContractsOptionContractsAndSwapContractsClassifiedAsInvestingActivities { get; set; }

        [JsonProperty("in-bse-fin:DividendsReceivedClassifiedAsInvestingActivities")]
        public InBseFinDividendsReceivedClassifiedAsInvestingActivities inbsefinDividendsReceivedClassifiedAsInvestingActivities { get; set; }

        [JsonProperty("in-bse-fin:InterestReceivedClassifiedAsInvestingActivities")]
        public InBseFinInterestReceivedClassifiedAsInvestingActivities inbsefinInterestReceivedClassifiedAsInvestingActivities { get; set; }

        [JsonProperty("in-bse-fin:IncomeTaxesPaidRefundClassifiedAsInvestingActivities")]
        public InBseFinIncomeTaxesPaidRefundClassifiedAsInvestingActivities inbsefinIncomeTaxesPaidRefundClassifiedAsInvestingActivities { get; set; }

        [JsonProperty("in-bse-fin:OtherInflowsOutflowsOfCashClassifiedAsInvestingActivities")]
        public InBseFinOtherInflowsOutflowsOfCashClassifiedAsInvestingActivities inbsefinOtherInflowsOutflowsOfCashClassifiedAsInvestingActivities { get; set; }

        [JsonProperty("in-bse-fin:ProceedsFromGovernmentGrantsClassifiedAsInvestingActivities")]
        public InBseFinProceedsFromGovernmentGrantsClassifiedAsInvestingActivities inbsefinProceedsFromGovernmentGrantsClassifiedAsInvestingActivities { get; set; }

        [JsonProperty("in-bse-fin:CashFlowsFromUsedInInvestingActivitiesBeforeExtraordinaryItems")]
        public InBseFinCashFlowsFromUsedInInvestingActivitiesBeforeExtraordinaryItems inbsefinCashFlowsFromUsedInInvestingActivitiesBeforeExtraordinaryItems { get; set; }

        [JsonProperty("in-bse-fin:ProceedsFromExtraordinaryItemsClassifiedAsInvestingActivities")]
        public InBseFinProceedsFromExtraordinaryItemsClassifiedAsInvestingActivities inbsefinProceedsFromExtraordinaryItemsClassifiedAsInvestingActivities { get; set; }

        [JsonProperty("in-bse-fin:PaymentForExtraordinaryItemsClassifiedAsInvestingActivities")]
        public InBseFinPaymentForExtraordinaryItemsClassifiedAsInvestingActivities inbsefinPaymentForExtraordinaryItemsClassifiedAsInvestingActivities { get; set; }

        [JsonProperty("in-bse-fin:CashFlowsFromUsedInInvestingActivities")]
        public InBseFinCashFlowsFromUsedInInvestingActivities inbsefinCashFlowsFromUsedInInvestingActivities { get; set; }

        [JsonProperty("in-bse-fin:ProceedsFromIssuingShares")]
        public InBseFinProceedsFromIssuingShares inbsefinProceedsFromIssuingShares { get; set; }

        [JsonProperty("in-bse-fin:ProceedsFromIssuingOtherEquityInstruments")]
        public InBseFinProceedsFromIssuingOtherEquityInstruments inbsefinProceedsFromIssuingOtherEquityInstruments { get; set; }

        [JsonProperty("in-bse-fin:ProceedsFromIssuingDebenturesNotesBondsEtc")]
        public InBseFinProceedsFromIssuingDebenturesNotesBondsEtc inbsefinProceedsFromIssuingDebenturesNotesBondsEtc { get; set; }

        [JsonProperty("in-bse-fin:ProceedsFromBorrowingsClassifiedAsFinancingActivities")]
        public InBseFinProceedsFromBorrowingsClassifiedAsFinancingActivities inbsefinProceedsFromBorrowingsClassifiedAsFinancingActivities { get; set; }

        [JsonProperty("in-bse-fin:RepaymentsOfBorrowingsClassifiedAsFinancingActivities")]
        public InBseFinRepaymentsOfBorrowingsClassifiedAsFinancingActivities inbsefinRepaymentsOfBorrowingsClassifiedAsFinancingActivities { get; set; }

        [JsonProperty("in-bse-fin:DividendsPaidClassifiedAsFinancingActivities")]
        public InBseFinDividendsPaidClassifiedAsFinancingActivities inbsefinDividendsPaidClassifiedAsFinancingActivities { get; set; }

        [JsonProperty("in-bse-fin:InterestPaidClassifiedAsFinancingActivities")]
        public InBseFinInterestPaidClassifiedAsFinancingActivities inbsefinInterestPaidClassifiedAsFinancingActivities { get; set; }

        [JsonProperty("in-bse-fin:IncomeTaxesPaidRefundClassifiedAsFinancingActivities")]
        public InBseFinIncomeTaxesPaidRefundClassifiedAsFinancingActivities inbsefinIncomeTaxesPaidRefundClassifiedAsFinancingActivities { get; set; }

        [JsonProperty("in-bse-fin:OtherInflowsOutflowsOfCashClassifiedAsFinancingActivities")]
        public InBseFinOtherInflowsOutflowsOfCashClassifiedAsFinancingActivities inbsefinOtherInflowsOutflowsOfCashClassifiedAsFinancingActivities { get; set; }

        [JsonProperty("in-bse-fin:CashFlowsFromUsedInFinancingActivitiesBeforeExtraordinaryItems")]
        public InBseFinCashFlowsFromUsedInFinancingActivitiesBeforeExtraordinaryItems inbsefinCashFlowsFromUsedInFinancingActivitiesBeforeExtraordinaryItems { get; set; }

        [JsonProperty("in-bse-fin:ProceedsFromExtraordinaryItemsClassifiedAsFinancingActivities")]
        public InBseFinProceedsFromExtraordinaryItemsClassifiedAsFinancingActivities inbsefinProceedsFromExtraordinaryItemsClassifiedAsFinancingActivities { get; set; }

        [JsonProperty("in-bse-fin:PaymentForExtraordinaryItemsClassifiedAsFinancingActivities")]
        public InBseFinPaymentForExtraordinaryItemsClassifiedAsFinancingActivities inbsefinPaymentForExtraordinaryItemsClassifiedAsFinancingActivities { get; set; }

        [JsonProperty("in-bse-fin:CashFlowsFromUsedInFinancingActivities")]
        public InBseFinCashFlowsFromUsedInFinancingActivities inbsefinCashFlowsFromUsedInFinancingActivities { get; set; }

        [JsonProperty("in-bse-fin:IncreaseDecreaseInCashAndCashEquivalentsBeforeEffectOfExchangeRateChanges")]
        public InBseFinIncreaseDecreaseInCashAndCashEquivalentsBeforeEffectOfExchangeRateChanges inbsefinIncreaseDecreaseInCashAndCashEquivalentsBeforeEffectOfExchangeRateChanges { get; set; }

        [JsonProperty("in-bse-fin:EffectOfExchangeRateChangesOnCashAndCashEquivalents")]
        public InBseFinEffectOfExchangeRateChangesOnCashAndCashEquivalents inbsefinEffectOfExchangeRateChangesOnCashAndCashEquivalents { get; set; }

        [JsonProperty("in-bse-fin:IncreaseDecreaseInCashAndCashEquivalents")]
        public InBseFinIncreaseDecreaseInCashAndCashEquivalents inbsefinIncreaseDecreaseInCashAndCashEquivalents { get; set; }

        [JsonProperty("in-bse-fin:CashAndCashEquivalentsCashFlowStatement")]
        public List<InBseFinCashAndCashEquivalentsCashFlowStatement> inbsefinCashAndCashEquivalentsCashFlowStatement { get; set; }
    }

    public class Xml
    {
        [JsonProperty("@version")]
        public string version { get; set; }

        [JsonProperty("@encoding")]
        public string encoding { get; set; }
    }


}
