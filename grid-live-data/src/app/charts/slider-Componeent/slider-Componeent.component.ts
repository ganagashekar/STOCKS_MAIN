import { Component, Input, SimpleChanges } from '@angular/core';
import { Options } from '@angular-slider/ngx-slider';
@Component({
  selector: 'app-slider-Componeent',
  templateUrl: './slider-Componeent.component.html',
  styleUrls: ['./slider-Componeent.component.css']
})

export class SliderComponeentComponent  {
  public sliders: SliderDetails[] = [];
  @Input()  public currentPrice :number |any
  @Input()  public min_len :number |any
  @Input()  public max_len :number |any
  @Input() public open : number| any

  options: Options = {
    floor: 0,
    ceil: 0,
    showSelectionBar: true,
    getSelectionBarColor: (value: number): string => {
      if(value >= this.open){
        return '#2AE02A'
      }
      else{
        return 'red';
      }
      // if (value <= 70.21) {
      //     return 'red';
      // }
      // if (value <= 71.30) {
      //     return 'orange';
      // }
      // if (value <= 72.50) {
      //     return 'yellow';
      // }
      // return '#2AE02A';
    }

  };
  constructor() { }

  ngOnInit() {
    this.options.floor=this.min_len;
    this.options.ceil=this.max_len;


  }

  sliderOptions(): Options {
    this.open=this.open;
    return {
      showSelectionBar: true,
      getSelectionBarColor: (value: number): string => {
        if(value >= this.open){
          return '#2AE02A'
        }
        else{
          return 'red';
        }
      },
      floor: this.min_len,
      ceil:this.max_len
    };
  }
  ngOnChanges(changes: any): void {

    // this.options.floor=changes.currentValue.min_len;
    // this.options.ceil=changes.currentValue.max_len;
    if (changes.currentPrice === null || changes.currentPrice  === undefined) {
    }
    else{

    //   if (changes.currentPrice.currentValue != changes.currentPrice.previousValue )
    //   {

    //  // this.value= changes.currentPrice.currentValue;
    //   }
    }

  }
}

interface SliderDetails {
  value: number;
  highValue: number;
  floor: number;
  ceil: number;
}
