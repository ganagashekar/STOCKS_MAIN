export interface Equities {
  symbol:     string;
  open:       number;
  last:       number;
  high:       number;
  low:        number;
  change:     number;
  bPrice:     number;
  bQty:       number;
  sPrice:     number;
  sQty:       number;
  ltq:        number;
  avgPrice:   number;
  quotes:     string;
  ttq:        number;
  totalBuyQt: number;
  totalSellQ: number;
  ttv:        string;
  trend:      string;
  lowerCktLm: number;
  upperCktLm: number;
  ltt:        string;
  close:      number;
  exchange:   string;
  stock_name: string;
  data:any;
  href:any;
  min:number;
  max:number;
  recmdtn:string;
  int_open :number;
  int_last:number;

  return1w :number;
  return1m :number;
  return3m : number;
  noofrec : string;
  beta :string ;

  eps:string;
  target:string;
  stockdetailshref:string;
  secId:string;
  isfavorite:number;
  volumeC:string;
}
