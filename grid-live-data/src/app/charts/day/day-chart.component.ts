import { Component, Input } from '@angular/core';
import { SeriesLine } from '@progress/kendo-angular-charts';
import { Observable } from 'rxjs';

@Component({
    selector: 'day-chart',
    templateUrl: './day-chart.template.html'
})
export class DayChartComponent {
  // @Input() public chartdata : Observable<number[]> | undefined;
    @Input() public data: number[] = [];
    @Input() public CurrentData : any;
    @Input() public dataopen: number[] = [];
    @Input() public dataavgPrice: number[] = [];
    @Input() public changePct: number = 0;

    public lineStyle: SeriesLine = { width: 2, style: 'smooth', color: '#4B5FFA' };

    // constructor() {
    //   setInterval(() => {
    //     // Clone the array
    //     const data= this.data.slice(0);

    //     // Produce one random value each 100ms
    //     data.push(Math.random()+1);

    //     // if (data.length > 10) {
    //     //   // Keep only 10 items in the array
    //     //   data.shift();
    //     // }

    //     // Replace with the new array instance
    //     this.data = data;
    //   }, 100);
    // }

    constructor() {

    }

    ngOnChanges(changes: any) {


      const dataopen=  this.dataopen.slice(0);
       const data= this.data.slice(0);
       const dataavgPrice=  this.dataavgPrice.slice(0);



      if(changes.CurrentData.currentValue.open < changes.CurrentData.currentValue.last){
        this.lineStyle.color="green";
      }
      else{
        this.lineStyle.color="red";
      }



      // dataopen.push(changes.CurrentData.currentValue.open);
       dataopen.push(changes.CurrentData.currentValue.open);
       data.push(changes.CurrentData.currentValue.last);
       dataavgPrice.push(changes.CurrentData.currentValue.avgPrice)
      // const dataopen=  this.dataopen.slice(0);
      // dataopen.push(70.5);
      // data.push(changes.changePct.currentValue)

      // // Produce one random val0ue each 100ms
      // //data.push((parseFloat(Math.random().toString())));

      // if (data.length > 1000) {
      //   // Keep only 10 items in the array
      //   data.shift();
      // }

      // // Replace with the new array instance
      this.dataopen=dataopen;
      this.data = data;
      this.dataavgPrice=dataavgPrice;




    }
}

