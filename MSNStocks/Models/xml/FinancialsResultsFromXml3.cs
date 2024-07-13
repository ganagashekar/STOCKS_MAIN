using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSNStocks.Models.xml3
{
    //internal class FinancialsResultsFromXml3
    //{
    //}


    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class InBseFinAmountOfItemThatWillBeReclassifiedToProfitAndLoss
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

    public class InBseFinAmountOfItemThatWillNotBeReclassifiedToProfitAndLoss
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

    public class InBseFinBasicEarningsLossPerShareFromContinuingAndDiscontinuedOperations
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

    public class InBseFinBasicEarningsLossPerShareFromContinuingOperations
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

    public class InBseFinBasicEarningsLossPerShareFromDiscontinuedOperations
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

    public class InBseFinChangesInInventoriesOfFinishedGoodsWorkInProgressAndStockInTrade
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

    public class InBseFinComprehensiveIncomeForThePeriod
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

    public class InBseFinComprehensiveIncomeForThePeriodAttributableToOwnersOfParent
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

    public class InBseFinComprehensiveIncomeForThePeriodAttributableToOwnersOfParentNonControllingInterests
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

    public class InBseFinCostOfMaterialsConsumed
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

    public class InBseFinCurrentTax
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

    public class InBseFinDebtEquityRatio
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

    public class InBseFinDebtServiceCoverageRatio
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

    public class InBseFinDeclarationOfUnmodifiedOpinionOrStatementOnImpactOfAuditQualification
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinDeferredTax
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

    public class InBseFinDepreciationDepletionAndAmortisationExpense
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

    public class InBseFinDescriptionOfItemThatWillBeReclassifiedToProfitAndLoss
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinDescriptionOfItemThatWillNotBeReclassifiedToProfitAndLoss
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinDescriptionOfOtherExpense
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

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

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinDilutedEarningsLossPerShareFromContinuingAndDiscontinuedOperations
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

    public class InBseFinDilutedEarningsLossPerShareFromContinuingOperations
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

    public class InBseFinDilutedEarningsLossPerShareFromDiscontinuedOperations
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

    public class InBseFinDisclosureOfNotesOnFinancialResultsExplanatoryTextBlock
    {
        [JsonProperty("@contextRef")]
        public string contextRef { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class InBseFinEmployeeBenefitExpense
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

    public class InBseFinExceptionalItemsBeforeTax
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

    public class InBseFinExpenses
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

    public class InBseFinFinanceCosts
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

    public class InBseFinIncomeTaxRelatingToItmesThatWillBeReclassifiedToProfitOrLoss
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

    public class InBseFinIncomeTaxRelatingToItmesThatWillNotBeReclassifiedToProfitOrLoss
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

    public class InBseFinInterestServiceCoverageRatio
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

    public class InBseFinNameOfTheCompany
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

    public class InBseFinNetMovementInRegulatoryDeferralAccountBalancesRelatedToProfitOrLossAndTheRelatedDeferredTaxMovement
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

    public class InBseFinOtherComprehensiveIncomeNetOfTaxes
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

    public class InBseFinOtherExpense
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

    public class InBseFinProfitBeforeExceptionalItemsAndTax
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

    public class InBseFinProfitBeforeTax
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

    public class InBseFinProfitLossForPeriod
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

    public class InBseFinProfitLossForPeriodFromContinuingOperations
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

    public class InBseFinProfitLossFromDiscontinuedOperationsAfterTax
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

    public class InBseFinProfitLossFromDiscontinuedOperationsBeforeTax
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

    public class InBseFinPurchasesOfStockInTrade
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

    public class InBseFinRevenueFromOperations
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

        [JsonProperty("@unitRef")]
        public string unitRef { get; set; }

        [JsonProperty("@decimals")]
        public string decimals { get; set; }

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

    public class InBseFinSegmentRevenueFromOperations
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

    public class InBseFinShareOfProfitLossOfAssociatesAndJointVenturesAccountedForUsingEquityMethod
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

    public class InBseFinTaxExpenseOfDiscontinuedOperations
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

    public class FinancialsResultsFromXml3
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

    public class XbrldiTypedMember
    {
        [JsonProperty("@dimension")]
        public string dimension { get; set; }
        public string ItemsThatWillNotBeReclassifiedToProfitAndLossDomain { get; set; }
        public string ItemsThatWillBeReclassifiedToProfitAndLossDomain { get; set; }
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

        [JsonProperty("xbrldi:typedMember")]
        public XbrldiTypedMember xbrlditypedMember { get; set; }
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

        [JsonProperty("in-bse-fin:NameOfTheCompany")]
        public InBseFinNameOfTheCompany inbsefinNameOfTheCompany { get; set; }

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

        [JsonProperty("in-bse-fin:StartTimeOfBoardMeeting")]
        public InBseFinStartTimeOfBoardMeeting inbsefinStartTimeOfBoardMeeting { get; set; }

        [JsonProperty("in-bse-fin:EndTimeOfBoardMeeting")]
        public InBseFinEndTimeOfBoardMeeting inbsefinEndTimeOfBoardMeeting { get; set; }

        [JsonProperty("in-bse-fin:DateOfStartOfBoardMeeting")]
        public InBseFinDateOfStartOfBoardMeeting inbsefinDateOfStartOfBoardMeeting { get; set; }

        [JsonProperty("in-bse-fin:DateOfEndOfBoardMeeting")]
        public InBseFinDateOfEndOfBoardMeeting inbsefinDateOfEndOfBoardMeeting { get; set; }

        [JsonProperty("in-bse-fin:DeclarationOfUnmodifiedOpinionOrStatementOnImpactOfAuditQualification")]
        public InBseFinDeclarationOfUnmodifiedOpinionOrStatementOnImpactOfAuditQualification inbsefinDeclarationOfUnmodifiedOpinionOrStatementOnImpactOfAuditQualification { get; set; }

        [JsonProperty("in-bse-fin:IsCompanyReportingMultisegmentOrSingleSegment")]
        public InBseFinIsCompanyReportingMultisegmentOrSingleSegment inbsefinIsCompanyReportingMultisegmentOrSingleSegment { get; set; }

        [JsonProperty("in-bse-fin:DateOfStartOfReportingPeriod")]
        public InBseFinDateOfStartOfReportingPeriod inbsefinDateOfStartOfReportingPeriod { get; set; }

        [JsonProperty("in-bse-fin:DateOfEndOfReportingPeriod")]
        public InBseFinDateOfEndOfReportingPeriod inbsefinDateOfEndOfReportingPeriod { get; set; }

        [JsonProperty("in-bse-fin:WhetherResultsAreAuditedOrUnaudited")]
        public InBseFinWhetherResultsAreAuditedOrUnaudited inbsefinWhetherResultsAreAuditedOrUnaudited { get; set; }

        [JsonProperty("in-bse-fin:NatureOfReportStandaloneConsolidated")]
        public InBseFinNatureOfReportStandaloneConsolidated inbsefinNatureOfReportStandaloneConsolidated { get; set; }

        [JsonProperty("in-bse-fin:RevenueFromOperations")]
        public InBseFinRevenueFromOperations inbsefinRevenueFromOperations { get; set; }

        [JsonProperty("in-bse-fin:OtherIncome")]
        public InBseFinOtherIncome inbsefinOtherIncome { get; set; }

        [JsonProperty("in-bse-fin:Income")]
        public InBseFinIncome inbsefinIncome { get; set; }

        [JsonProperty("in-bse-fin:CostOfMaterialsConsumed")]
        public InBseFinCostOfMaterialsConsumed inbsefinCostOfMaterialsConsumed { get; set; }

        [JsonProperty("in-bse-fin:PurchasesOfStockInTrade")]
        public InBseFinPurchasesOfStockInTrade inbsefinPurchasesOfStockInTrade { get; set; }

        [JsonProperty("in-bse-fin:ChangesInInventoriesOfFinishedGoodsWorkInProgressAndStockInTrade")]
        public InBseFinChangesInInventoriesOfFinishedGoodsWorkInProgressAndStockInTrade inbsefinChangesInInventoriesOfFinishedGoodsWorkInProgressAndStockInTrade { get; set; }

        [JsonProperty("in-bse-fin:EmployeeBenefitExpense")]
        public InBseFinEmployeeBenefitExpense inbsefinEmployeeBenefitExpense { get; set; }

        [JsonProperty("in-bse-fin:FinanceCosts")]
        public InBseFinFinanceCosts inbsefinFinanceCosts { get; set; }

        [JsonProperty("in-bse-fin:DepreciationDepletionAndAmortisationExpense")]
        public InBseFinDepreciationDepletionAndAmortisationExpense inbsefinDepreciationDepletionAndAmortisationExpense { get; set; }

        [JsonProperty("in-bse-fin:OtherExpenses")]
        public List<InBseFinOtherExpense> inbsefinOtherExpenses { get; set; }

        [JsonProperty("in-bse-fin:Expenses")]
        public InBseFinExpenses inbsefinExpenses { get; set; }

        [JsonProperty("in-bse-fin:ProfitBeforeExceptionalItemsAndTax")]
        public InBseFinProfitBeforeExceptionalItemsAndTax inbsefinProfitBeforeExceptionalItemsAndTax { get; set; }

        [JsonProperty("in-bse-fin:ExceptionalItemsBeforeTax")]
        public InBseFinExceptionalItemsBeforeTax inbsefinExceptionalItemsBeforeTax { get; set; }

        [JsonProperty("in-bse-fin:ProfitBeforeTax")]
        public InBseFinProfitBeforeTax inbsefinProfitBeforeTax { get; set; }

        [JsonProperty("in-bse-fin:CurrentTax")]
        public InBseFinCurrentTax inbsefinCurrentTax { get; set; }

        [JsonProperty("in-bse-fin:DeferredTax")]
        public InBseFinDeferredTax inbsefinDeferredTax { get; set; }

        [JsonProperty("in-bse-fin:TaxExpense")]
        public InBseFinTaxExpense inbsefinTaxExpense { get; set; }

        [JsonProperty("in-bse-fin:NetMovementInRegulatoryDeferralAccountBalancesRelatedToProfitOrLossAndTheRelatedDeferredTaxMovement")]
        public InBseFinNetMovementInRegulatoryDeferralAccountBalancesRelatedToProfitOrLossAndTheRelatedDeferredTaxMovement inbsefinNetMovementInRegulatoryDeferralAccountBalancesRelatedToProfitOrLossAndTheRelatedDeferredTaxMovement { get; set; }

        [JsonProperty("in-bse-fin:ProfitLossForPeriodFromContinuingOperations")]
        public InBseFinProfitLossForPeriodFromContinuingOperations inbsefinProfitLossForPeriodFromContinuingOperations { get; set; }

        [JsonProperty("in-bse-fin:ProfitLossFromDiscontinuedOperationsBeforeTax")]
        public InBseFinProfitLossFromDiscontinuedOperationsBeforeTax inbsefinProfitLossFromDiscontinuedOperationsBeforeTax { get; set; }

        [JsonProperty("in-bse-fin:TaxExpenseOfDiscontinuedOperations")]
        public InBseFinTaxExpenseOfDiscontinuedOperations inbsefinTaxExpenseOfDiscontinuedOperations { get; set; }

        [JsonProperty("in-bse-fin:ProfitLossFromDiscontinuedOperationsAfterTax")]
        public InBseFinProfitLossFromDiscontinuedOperationsAfterTax inbsefinProfitLossFromDiscontinuedOperationsAfterTax { get; set; }

        [JsonProperty("in-bse-fin:ShareOfProfitLossOfAssociatesAndJointVenturesAccountedForUsingEquityMethod")]
        public InBseFinShareOfProfitLossOfAssociatesAndJointVenturesAccountedForUsingEquityMethod inbsefinShareOfProfitLossOfAssociatesAndJointVenturesAccountedForUsingEquityMethod { get; set; }

        [JsonProperty("in-bse-fin:ProfitLossForPeriod")]
        public InBseFinProfitLossForPeriod inbsefinProfitLossForPeriod { get; set; }

        [JsonProperty("in-bse-fin:OtherComprehensiveIncomeNetOfTaxes")]
        public InBseFinOtherComprehensiveIncomeNetOfTaxes inbsefinOtherComprehensiveIncomeNetOfTaxes { get; set; }

        [JsonProperty("in-bse-fin:ComprehensiveIncomeForThePeriod")]
        public InBseFinComprehensiveIncomeForThePeriod inbsefinComprehensiveIncomeForThePeriod { get; set; }

        [JsonProperty("in-bse-fin:ComprehensiveIncomeForThePeriodAttributableToOwnersOfParent")]
        public InBseFinComprehensiveIncomeForThePeriodAttributableToOwnersOfParent inbsefinComprehensiveIncomeForThePeriodAttributableToOwnersOfParent { get; set; }

        [JsonProperty("in-bse-fin:ComprehensiveIncomeForThePeriodAttributableToOwnersOfParentNonControllingInterests")]
        public InBseFinComprehensiveIncomeForThePeriodAttributableToOwnersOfParentNonControllingInterests inbsefinComprehensiveIncomeForThePeriodAttributableToOwnersOfParentNonControllingInterests { get; set; }

        [JsonProperty("in-bse-fin:PaidUpValueOfEquityShareCapital")]
        public InBseFinPaidUpValueOfEquityShareCapital inbsefinPaidUpValueOfEquityShareCapital { get; set; }

        [JsonProperty("in-bse-fin:FaceValueOfEquityShareCapital")]
        public InBseFinFaceValueOfEquityShareCapital inbsefinFaceValueOfEquityShareCapital { get; set; }

        [JsonProperty("in-bse-fin:BasicEarningsLossPerShareFromContinuingOperations")]
        public InBseFinBasicEarningsLossPerShareFromContinuingOperations inbsefinBasicEarningsLossPerShareFromContinuingOperations { get; set; }

        [JsonProperty("in-bse-fin:DilutedEarningsLossPerShareFromContinuingOperations")]
        public InBseFinDilutedEarningsLossPerShareFromContinuingOperations inbsefinDilutedEarningsLossPerShareFromContinuingOperations { get; set; }

        [JsonProperty("in-bse-fin:BasicEarningsLossPerShareFromDiscontinuedOperations")]
        public InBseFinBasicEarningsLossPerShareFromDiscontinuedOperations inbsefinBasicEarningsLossPerShareFromDiscontinuedOperations { get; set; }

        [JsonProperty("in-bse-fin:DilutedEarningsLossPerShareFromDiscontinuedOperations")]
        public InBseFinDilutedEarningsLossPerShareFromDiscontinuedOperations inbsefinDilutedEarningsLossPerShareFromDiscontinuedOperations { get; set; }

        [JsonProperty("in-bse-fin:BasicEarningsLossPerShareFromContinuingAndDiscontinuedOperations")]
        public InBseFinBasicEarningsLossPerShareFromContinuingAndDiscontinuedOperations inbsefinBasicEarningsLossPerShareFromContinuingAndDiscontinuedOperations { get; set; }

        [JsonProperty("in-bse-fin:DilutedEarningsLossPerShareFromContinuingAndDiscontinuedOperations")]
        public InBseFinDilutedEarningsLossPerShareFromContinuingAndDiscontinuedOperations inbsefinDilutedEarningsLossPerShareFromContinuingAndDiscontinuedOperations { get; set; }

        [JsonProperty("in-bse-fin:DebtEquityRatio")]
        public InBseFinDebtEquityRatio inbsefinDebtEquityRatio { get; set; }

        [JsonProperty("in-bse-fin:DebtServiceCoverageRatio")]
        public InBseFinDebtServiceCoverageRatio inbsefinDebtServiceCoverageRatio { get; set; }

        [JsonProperty("in-bse-fin:InterestServiceCoverageRatio")]
        public InBseFinInterestServiceCoverageRatio inbsefinInterestServiceCoverageRatio { get; set; }

        [JsonProperty("in-bse-fin:DisclosureOfNotesOnFinancialResultsExplanatoryTextBlock")]
        public InBseFinDisclosureOfNotesOnFinancialResultsExplanatoryTextBlock inbsefinDisclosureOfNotesOnFinancialResultsExplanatoryTextBlock { get; set; }

        [JsonProperty("in-bse-fin:DescriptionOfOtherExpenses")]
        public List<InBseFinDescriptionOfOtherExpense> inbsefinDescriptionOfOtherExpenses { get; set; }

        [JsonProperty("in-bse-fin:DescriptionOfReportableSegment")]
        public List<InBseFinDescriptionOfReportableSegment> inbsefinDescriptionOfReportableSegment { get; set; }

        [JsonProperty("in-bse-fin:SegmentRevenue")]
        public List<InBseFinSegmentRevenue> inbsefinSegmentRevenue { get; set; }

        [JsonProperty("in-bse-fin:SegmentRevenueFromOperations")]
        public InBseFinSegmentRevenueFromOperations inbsefinSegmentRevenueFromOperations { get; set; }

        [JsonProperty("in-bse-fin:SegmentProfitLossBeforeTaxAndFinanceCosts")]
        public List<InBseFinSegmentProfitLossBeforeTaxAndFinanceCost> inbsefinSegmentProfitLossBeforeTaxAndFinanceCosts { get; set; }

        [JsonProperty("in-bse-fin:SegmentProfitBeforeTax")]
        public InBseFinSegmentProfitBeforeTax inbsefinSegmentProfitBeforeTax { get; set; }

        [JsonProperty("in-bse-fin:SegmentAssets")]
        public List<InBseFinSegmentAsset> inbsefinSegmentAssets { get; set; }

        [JsonProperty("in-bse-fin:SegmentLiabilities")]
        public List<InBseFinSegmentLiability> inbsefinSegmentLiabilities { get; set; }

        [JsonProperty("in-bse-fin:DescriptionOfItemThatWillNotBeReclassifiedToProfitAndLoss")]
        public InBseFinDescriptionOfItemThatWillNotBeReclassifiedToProfitAndLoss inbsefinDescriptionOfItemThatWillNotBeReclassifiedToProfitAndLoss { get; set; }

        [JsonProperty("in-bse-fin:AmountOfItemThatWillNotBeReclassifiedToProfitAndLoss")]
        public List<InBseFinAmountOfItemThatWillNotBeReclassifiedToProfitAndLoss> inbsefinAmountOfItemThatWillNotBeReclassifiedToProfitAndLoss { get; set; }

        [JsonProperty("in-bse-fin:IncomeTaxRelatingToItmesThatWillNotBeReclassifiedToProfitOrLoss")]
        public InBseFinIncomeTaxRelatingToItmesThatWillNotBeReclassifiedToProfitOrLoss inbsefinIncomeTaxRelatingToItmesThatWillNotBeReclassifiedToProfitOrLoss { get; set; }

        [JsonProperty("in-bse-fin:DescriptionOfItemThatWillBeReclassifiedToProfitAndLoss")]
        public InBseFinDescriptionOfItemThatWillBeReclassifiedToProfitAndLoss inbsefinDescriptionOfItemThatWillBeReclassifiedToProfitAndLoss { get; set; }

        [JsonProperty("in-bse-fin:AmountOfItemThatWillBeReclassifiedToProfitAndLoss")]
        public List<InBseFinAmountOfItemThatWillBeReclassifiedToProfitAndLoss> inbsefinAmountOfItemThatWillBeReclassifiedToProfitAndLoss { get; set; }

        [JsonProperty("in-bse-fin:IncomeTaxRelatingToItmesThatWillBeReclassifiedToProfitOrLoss")]
        public InBseFinIncomeTaxRelatingToItmesThatWillBeReclassifiedToProfitOrLoss inbsefinIncomeTaxRelatingToItmesThatWillBeReclassifiedToProfitOrLoss { get; set; }
    }

    public class Xml
    {
        [JsonProperty("@version")]
        public string version { get; set; }

        [JsonProperty("@encoding")]
        public string encoding { get; set; }
    }




}
