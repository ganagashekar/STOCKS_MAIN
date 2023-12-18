import { Component, Input, OnInit } from '@angular/core';
import { SeriesLine } from '@progress/kendo-angular-charts';

@Component({
  selector: 'app-week-chart',
  templateUrl: './week-chart.component.html',
  styleUrls: ['./week-chart.component.css']
})
export class WeekChartComponent implements OnInit {
  public seriesData: number[] = [0.5,0.4,0.3,0.9,0.5,0.4,0.3,0.9,0.5,0.4,0.3,0.9,0.5,0.4,0.3,0.9,0.5,0.4,0.3,0.9,0.5,0.4,0.3,0.9,0.5,0.4,0.3,0.9,0.5,0.4,0.3,0.9,0.5,0.4,0.3,0.9,0.5,0.4,0.3,0.9,0.5,0.4,0.3,0.9,0.5,0.4,0.3,0.9];
  @Input() public data: number[] = [1,2,3,4];
    @Input() public CurrentData : any;
    @Input() public dataopen: number[] = [];
    @Input() public dataavgPrice: number[] = [];
    @Input() public changePct: number = 0;

    public lineStyle: SeriesLine = { width: 2, style: 'smooth', color: '#4B5FFA' };
  constructor() { }

  ngOnInit(): void {
  }

}
