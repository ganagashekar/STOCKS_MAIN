import { Component, Input } from '@angular/core';
import { PlotBand, SeriesLine } from '@progress/kendo-angular-charts';
import { Observable } from 'rxjs';
import { Path, Group, Text } from "@progress/kendo-drawing";
import { Rect } from "@progress/kendo-drawing/geometry";
import { RenderEvent } from "@progress/kendo-angular-charts";
@Component({
    selector: 'day-chart',
    templateUrl: './day-chart.template.html'
})
export class DayChartComponent {
  // @Input() public chartdata : Observable<number[]> | undefined;
   @Input() public min: number = 0;
   @Input() public max: number = 0;
    @Input() public data: number[] = [];
    @Input() public CurrentData : any;
    @Input() public dataopen: number[] = [];
    @Input() public dataavgPrice: number[] = [];
    @Input() public changePct: number = 0;
    @Input() public last: number = 0;
    @Input() public open: number = 0;



    public lineStyle: SeriesLine = { width: 2, style: 'normal', color: '#4B5FFA' };

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
    public valuePlotBands: PlotBand[] = [
    {
      from: parseInt(this.open.toString()),
      to: parseInt(this.last.toString()),
      color: "#000000",
      opacity: 15,
      // label: {
      //   // // text: "Green Zone",
      //   // // font: "18px sans-serif",
      //   // color: "#444",

      // },
    },
  ]
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


      this.valuePlotBands


    }

    public onRender = (args: RenderEvent): void => {
      const chart = args.sender;

      // get the axes
      const valueAxis = chart.findAxisByName("valueAxis");
      const categoryAxis = chart.findAxisByName("categoryAxis");

      // get the coordinates of the value at which the plot band will be rendered
      const valueSlot = valueAxis.slot(this.last) as Rect;

      // get the coordinates of the entire category axis range
      const range = categoryAxis.range();
      const categorySlot = categoryAxis.slot(
        range.min as number,
        range.max as number
      ) as Rect;

      // draw the plot band based on the found coordinates
      const line = new Path({
        stroke: {
          color: "black",
          width: 0.5,
        },
      })
        .moveTo(valueSlot.origin)
        .lineTo(categorySlot.topRight().x, valueSlot.origin.y);

      const label = new Text( this.last.toString(), [0, 0], {
        fill: {
          color: "red",
        },

      });
      const bbox = label.bbox();
      label.position([
        categorySlot.topRight().x - bbox.size.width,
        valueSlot.origin.y - bbox.size.height,
      ]);

      const group = new Group();
      group.append(line, label);

      // Draw on the Chart surface to overlay the series
      chart.surface.draw(group);

      // Or as a first element in the pane to appear behind the series
      // chart.findPaneByIndex(0).visual.insert(0, group);
    };
}

