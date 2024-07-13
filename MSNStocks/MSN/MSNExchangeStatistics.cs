using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSNStocks.MSN
{

    // Root myDeserializedClass = JsonConvert.DeserializeObject<List<Root>>(myJsonResponse);
    // Root myDeserializedClass = JsonConvert.DeserializeObject<List<Root>>(myJsonResponse);
    // Root myDeserializedClass = JsonConvert.DeserializeObject<List<Root>>(myJsonResponse);
    public class Active
    {
        public string instrumentId { get; set; }
        public string displayName { get; set; }
        public string shortName { get; set; }
        public string exchangeId { get; set; }
        public string exchangeCode { get; set; }
        public string securityType { get; set; }
        public string symbol { get; set; }
    }

    public class ArAe
    {
        public string displayName { get; set; }
    }

    public class ArEg
    {
        public string displayName { get; set; }
    }

    public class ArSa
    {
        public string displayName { get; set; }
    }

    public class BnIn
    {
        public string displayName { get; set; }
    }

    public class CsCz
    {
        public string displayName { get; set; }
    }

    public class DaDk
    {
        public string displayName { get; set; }
    }

    public class DeAt
    {
        public string displayName { get; set; }
    }

    public class DeCh
    {
        public string displayName { get; set; }
    }

    public class DeDe
    {
        public string displayName { get; set; }
    }

    public class ElGr
    {
        public string displayName { get; set; }
    }

    public class EnAe
    {
        public string displayName { get; set; }
    }

    public class EnAu
    {
        public string displayName { get; set; }
    }

    public class EnCa
    {
        public string displayName { get; set; }
    }

    public class EnGb
    {
        public string displayName { get; set; }
    }

    public class EnIe
    {
        public string displayName { get; set; }
    }

    public class EnIn
    {
        public string displayName { get; set; }
    }

    public class EnMy
    {
        public string displayName { get; set; }
    }

    public class EnNz
    {
        public string displayName { get; set; }
    }

    public class EnPh
    {
        public string displayName { get; set; }
    }

    public class EnSg
    {
        public string displayName { get; set; }
    }

    public class EnUs
    {
        public string displayName { get; set; }
    }

    public class EnXl
    {
        public string displayName { get; set; }
    }

    public class EnZa
    {
        public string displayName { get; set; }
    }

    public class EsAr
    {
        public string displayName { get; set; }
    }

    public class EsCl
    {
        public string displayName { get; set; }
    }

    public class EsCo
    {
        public string displayName { get; set; }
    }

    public class EsEs
    {
        public string displayName { get; set; }
    }

    public class EsMx
    {
        public string displayName { get; set; }
    }

    public class EsPe
    {
        public string displayName { get; set; }
    }

    public class EsUs
    {
        public string displayName { get; set; }
    }

    public class EsVe
    {
        public string displayName { get; set; }
    }

    public class EsXl
    {
        public string displayName { get; set; }
    }

    public class FiFi
    {
        public string displayName { get; set; }
    }

    public class FrBe
    {
        public string displayName { get; set; }
    }

    public class FrCa
    {
        public string displayName { get; set; }
    }

    public class FrCh
    {
        public string displayName { get; set; }
    }

    public class FrFr
    {
        public string displayName { get; set; }
    }

    public class FrXl
    {
        public string displayName { get; set; }
    }

    public class Gainer
    {
        public string instrumentId { get; set; }
        public string displayName { get; set; }
        public string shortName { get; set; }
        public string exchangeId { get; set; }
        public string exchangeCode { get; set; }
        public string securityType { get; set; }
        public string symbol { get; set; }
    }

    public class HeIl
    {
        public string displayName { get; set; }
    }

    public class HiIn
    {
        public string displayName { get; set; }
    }

    public class HuHu
    {
        public string displayName { get; set; }
    }

    public class IdId
    {
        public string displayName { get; set; }
    }

    public class ItIt
    {
        public string displayName { get; set; }
    }

    public class JaJp
    {
        public string displayName { get; set; }
    }

    public class KoKr
    {
        public string displayName { get; set; }
    }

    public class LocalizedAttributes
    {
        [JsonProperty("vi-vn")]
        public ViVn vivn { get; set; }

        [JsonProperty("en-my")]
        public EnMy enmy { get; set; }

        [JsonProperty("en-in")]
        public EnIn enin { get; set; }

        [JsonProperty("en-gb")]
        public EnGb engb { get; set; }

        [JsonProperty("tr-tr")]
        public TrTr trtr { get; set; }

        [JsonProperty("ar-sa")]
        public ArSa arsa { get; set; }

        [JsonProperty("de-ch")]
        public DeCh dech { get; set; }

        [JsonProperty("da-dk")]
        public DaDk dadk { get; set; }

        [JsonProperty("ru-ru")]
        public RuRu ruru { get; set; }

        [JsonProperty("es-us")]
        public EsUs esus { get; set; }

        [JsonProperty("sv-se")]
        public SvSe svse { get; set; }

        [JsonProperty("ar-ae")]
        public ArAe arae { get; set; }

        [JsonProperty("hi-in")]
        public HiIn hiin { get; set; }

        [JsonProperty("en-us")]
        public EnUs enus { get; set; }

        [JsonProperty("fr-ch")]
        public FrCh frch { get; set; }

        [JsonProperty("es-co")]
        public EsCo esco { get; set; }

        [JsonProperty("he-il")]
        public HeIl heil { get; set; }

        [JsonProperty("id-id")]
        public IdId idid { get; set; }

        [JsonProperty("pt-br")]
        public PtBr ptbr { get; set; }

        [JsonProperty("es-xl")]
        public EsXl esxl { get; set; }

        [JsonProperty("en-ae")]
        public EnAe enae { get; set; }

        [JsonProperty("es-ve")]
        public EsVe esve { get; set; }

        [JsonProperty("de-de")]
        public DeDe dede { get; set; }

        [JsonProperty("ko-kr")]
        public KoKr kokr { get; set; }

        [JsonProperty("bn-in")]
        public BnIn bnin { get; set; }

        [JsonProperty("es-es")]
        public EsEs eses { get; set; }

        [JsonProperty("es-cl")]
        public EsCl escl { get; set; }

        [JsonProperty("nl-nl")]
        public NlNl nlnl { get; set; }

        [JsonProperty("en-sg")]
        public EnSg ensg { get; set; }

        [JsonProperty("en-ph")]
        public EnPh enph { get; set; }

        [JsonProperty("en-ca")]
        public EnCa enca { get; set; }

        [JsonProperty("es-mx")]
        public EsMx esmx { get; set; }

        [JsonProperty("de-at")]
        public DeAt deat { get; set; }

        [JsonProperty("en-xl")]
        public EnXl enxl { get; set; }

        [JsonProperty("fr-fr")]
        public FrFr frfr { get; set; }

        [JsonProperty("zh-hk")]
        public ZhHk zhhk { get; set; }

        [JsonProperty("zh-tw")]
        public ZhTw zhtw { get; set; }

        [JsonProperty("cs-cz")]
        public CsCz cscz { get; set; }

        [JsonProperty("zh-cn")]
        public ZhCn zhcn { get; set; }

        [JsonProperty("ja-jp")]
        public JaJp jajp { get; set; }

        [JsonProperty("hu-hu")]
        public HuHu huhu { get; set; }

        [JsonProperty("ar-eg")]
        public ArEg areg { get; set; }

        [JsonProperty("nb-no")]
        public NbNo nbno { get; set; }

        [JsonProperty("es-ar")]
        public EsAr esar { get; set; }

        [JsonProperty("el-gr")]
        public ElGr elgr { get; set; }

        [JsonProperty("pt-pt")]
        public PtPt ptpt { get; set; }

        [JsonProperty("en-nz")]
        public EnNz ennz { get; set; }

        [JsonProperty("fr-xl")]
        public FrXl frxl { get; set; }

        [JsonProperty("fr-be")]
        public FrBe frbe { get; set; }

        [JsonProperty("en-ie")]
        public EnIe enie { get; set; }

        [JsonProperty("es-pe")]
        public EsPe espe { get; set; }

        [JsonProperty("pl-pl")]
        public PlPl plpl { get; set; }

        [JsonProperty("fi-fi")]
        public FiFi fifi { get; set; }

        [JsonProperty("en-za")]
        public EnZa enza { get; set; }

        [JsonProperty("th-th")]
        public ThTh thth { get; set; }

        [JsonProperty("nl-be")]
        public NlBe nlbe { get; set; }

        [JsonProperty("te-in")]
        public TeIn tein { get; set; }

        [JsonProperty("mr-in")]
        public MrIn mrin { get; set; }

        [JsonProperty("fr-ca")]
        public FrCa frca { get; set; }

        [JsonProperty("it-it")]
        public ItIt itit { get; set; }

        [JsonProperty("en-au")]
        public EnAu enau { get; set; }
    }

    public class Loser
    {
        public string instrumentId { get; set; }
        public string displayName { get; set; }
        public string shortName { get; set; }
        public string exchangeId { get; set; }
        public string exchangeCode { get; set; }
        public string securityType { get; set; }
        public string symbol { get; set; }
    }

    public class MrIn
    {
        public string displayName { get; set; }
    }

    public class NbNo
    {
        public string displayName { get; set; }
    }

    public class NlBe
    {
        public string displayName { get; set; }
    }

    public class NlNl
    {
        public string displayName { get; set; }
    }

    public class PlPl
    {
        public string displayName { get; set; }
    }

    public class PtBr
    {
        public string displayName { get; set; }
    }

    public class PtPt
    {
        public string displayName { get; set; }
    }

    public class MSNExchangeStatisticsModel
    {
        public string exchangeId { get; set; }
        public string exchangeCode { get; set; }
        public string displayName { get; set; }
        public LocalizedAttributes localizedAttributes { get; set; }
        public List<Active> active { get; set; }
        public List<Gainer> gainers { get; set; }
        public List<Loser> losers { get; set; }
        public DateTime timeLastUpdated { get; set; }
        public string _p { get; set; }
        public string id { get; set; }
        public string _t { get; set; }
    }

    public class RuRu
    {
        public string displayName { get; set; }
    }

    public class SvSe
    {
        public string displayName { get; set; }
    }

    public class TeIn
    {
        public string displayName { get; set; }
    }

    public class ThTh
    {
        public string displayName { get; set; }
    }

    public class TrTr
    {
        public string displayName { get; set; }
    }

    public class ViVn
    {
        public string displayName { get; set; }
    }

    public class ZhCn
    {
        public string displayName { get; set; }
    }

    public class ZhHk
    {
        public string displayName { get; set; }
    }

    public class ZhTw
    {
        public string displayName { get; set; }
    }






}

