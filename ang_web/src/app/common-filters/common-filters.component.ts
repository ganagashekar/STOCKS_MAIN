import { Component, OnInit,ViewChild } from '@angular/core';
import { SignalrService } from '../services/signalr.service';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';
import { MatSelect, MatSelectChange } from '@angular/material/select';
import { StocksService } from '../services/stocks.service';
import { HttpClient } from '@angular/common/http';
import { dropdownModel } from '../models/transaction.model';

import { FormControl, FormGroup,ReactiveFormsModule,FormsModule } from '@angular/forms';
import { MatOption } from '@angular/material/core';



interface ColumnList {
  value: string;
  viewValue: string;
}


@Component({
  selector: 'app-common-filters',
  templateUrl: './common-filters.component.html',
  styleUrls: ['./common-filters.component.css']
})

export class CommonFiltersComponent implements OnInit {

  @ViewChild('selectSector') selectSector: MatSelect | any;

  @ViewChild('selectindustry') selectindustry: MatSelect | any;

  @ViewChild('selectgroup') selectgroup: MatSelect | any;

  @ViewChild('selectSubgroup') selectSubgroup: MatSelect | any;


  SectorallSelected=false;

  industryallSelected=false;

  groupallSelected=false;

  subgroupallSelected=false;

  CKTAllSelected=false;

  valuemin=0;
  valuemax=0;
  selected_Operator:any;


  html: SafeHtml | any;
  selectedTop : number |any ;
  SelectedDate : string | any ;

  CKTName = new FormControl();
  CKTNameList :  dropdownModel[] =[{text :"",value :""},{text :"upperCktLm",value :"upperCktLm"},{text :"lowerCktLm",value :"lowerCktLm"}];
  selectedCKTName : any ;

  sectorName = new FormControl();
  sectorNameList :  dropdownModel[] = [];
  selectedsectorName : any ;

  industryNewName = new FormControl();
  industryNewNameList :  dropdownModel[] = [];
  selectedIndustryNewName : any ;

  groupName = new FormControl();
  groupNameList :  dropdownModel[] = [];
  selectedgroupName : any ;


  SubgroupName = new FormControl();
  SubgroupNameList :  dropdownModel[] = [];
  selectedSubgroupName : any ;

  ColumnLists: ColumnList[] = [
    {value: 'last', viewValue: 'last'},
    {value: 'close', viewValue: 'close'},
    {value: 'change', viewValue: 'change'},
    {value: 'ttv', viewValue: 'ttv'},
    {value: 'open', viewValue: 'open'},
    {value: 'high-Low', viewValue: 'high-Low'},
    {value: 'Symbol', viewValue: 'Symbol'},

  ];

  OperatorList: ColumnList[] = [
    {value: '>', viewValue: '>'},
    {value: '<', viewValue: '<'},
    {value: 'Range', viewValue: 'Range'},
    {value: '=', viewValue: '='},


  ];
  // sectorNameform: FormGroup;



  constructor(public _sanitizer: DomSanitizer,public signalRService: SignalrService, public stocksService: StocksService, private http: HttpClient) {

    // this.sectorNameform = new FormGroup({
    //   sector: new FormControl(this.sectorNameList)
    // });
    this.signalRService.connection
              .invoke('GetSectorName')
              .catch((error: any) => {
                console.log(`GetSectorName error: ${error}`);
                alert('GetSectorName error!, see console for details.');
          });

                this.signalRService.connection.on("SendSectorName",(val :[any]) => {
                debugger;
                this.sectorNameList = val;
                })


                this.signalRService.connection.on("SendIndustryNewName",(val :[any]) => {
                debugger;
                this.industryNewNameList = val;
                })


                this.signalRService.connection.on("SendGroupName",(val :[any]) => {
                debugger;
                this.groupNameList = val;
                })


                this.signalRService.connection.on("SendSubgroupName",(val :[any]) => {
                debugger;
                this.SubgroupNameList = val;
                })

   }

  ngOnInit(): void {


  }

  GetPivotData(text:any) {
    this.fetchpivotdata();
  }

  GetPivotDataWatchList(text:any) {
    this.fetchpivotdataWatchList();
  }


  toggleAllSelection_selectSector() {
    if (this.SectorallSelected) {
      this.selectSector.options.forEach((item: MatOption) => item.select());
    } else {
      this.selectSector.options.forEach((item: MatOption) => item.deselect());
    }
  }

  toggleAllSelection_industry() {
    if (this.industryallSelected) {
      this.selectindustry.options.forEach((item: MatOption) => item.select());
    } else {
      this.selectindustry.options.forEach((item: MatOption) => item.deselect());
    }
  }


  toggleAllSelection_selectgroup() {
    if (this.groupallSelected) {
      this.selectgroup.options.forEach((item: MatOption) => item.select());
    } else {
      this.selectgroup.options.forEach((item: MatOption) => item.deselect());
    }
  }


  toggleAllSelection_selectSubgroup() {
    if (this.subgroupallSelected) {
      this.selectSubgroup.options.forEach((item: MatOption) => item.select());
    } else {
      this.selectSubgroup.options.forEach((item: MatOption) => item.deselect());
    }
  }





  selectedCKTChange(event: any) {
    debugger;

    this.selected_Operator=event;



  }

  selectedsectorNameChange(event: any) {
    debugger;

    this.industryNewNameList = [];
    this.groupNameList = [];
    this.SubgroupNameList = [];
    this.signalRService.connection
              .invoke('GetIndustryNewName',event.join(','))
              .catch((error: any) => {
                console.log(`GetSectorName error: ${error}`);
                alert('GetSectorName error!, see console for details.');
          });


  }
  selectedgroupNameNameChange(event: any) {
    debugger;

    this.SubgroupNameList = [];
    this.signalRService.connection
              .invoke('GetSubgroupName',event.join(','))
              .catch((error: any) => {
                console.log(`GetSectorName error: ${error}`);
                alert('GetSectorName error!, see console for details.');
          });


  }

  selectedsubgroupNameNameChange(event: any) {
    debugger;

    //this.fetchpivotdata();


  }


  selectedIndustryNameChange(event: any) {
    debugger;


    this.groupNameList = [];
    this.SubgroupNameList = [];
    this.signalRService.connection
              .invoke('GetGroupName',event.join(','))
              .catch((error: any) => {
                console.log(`GetSectorName error: ${error}`);
                alert('GetSectorName error!, see console for details.');
          });


  }
   onDateChange(event: any ): void {
    debugger;
    this.SelectedDate = event.value.toUTCString();
   // this.fetchpivotdata();

  }

  onvalueminChange(event: any ): void {
    debugger;
    this.valuemin = event.value;
   // this.fetchpivotdata();

  }

  onvaluemaxChange(event: any ): void {
    debugger;
    this.valuemax = event.value;
   // this.fetchpivotdata();

  }

  selected(event: MatSelectChange) {
    debugger;
    this.selectedTop=event;
   // this.fetchpivotdata();
  }


  selectedOperator(event: MatSelectChange) {
    debugger;
    this.selected_Operator=event;
   // this.fetchpivotdata();
  }

  fetchpivotdata() :void {
   debugger;
    var cpy_selectedgroupName=this.selectedgroupName;
   var  cpy_sselectedSubgroupName=this.selectedSubgroupName;

    if(this.groupallSelected) {
      cpy_selectedgroupName=[];
    }
    if(this.subgroupallSelected) {
      cpy_sselectedSubgroupName=[];
    }
    this.signalRService.connection
              .invoke('GetPivotData',this.SelectedDate,this.selectedTop,cpy_selectedgroupName.join(','),cpy_sselectedSubgroupName.join(','),this.selectedCKTName
              ,this.selected_Operator,this.valuemin,this.valuemax,0
              )
              .catch((error: any) => {
                console.log(`GetPivotData error: ${error}`);
                alert('GetPivotData error!, see console for details.');
          });
  }


  fetchpivotdataWatchList() :void {
    debugger;
     var cpy_selectedgroupName=this.selectedgroupName;
    var  cpy_sselectedSubgroupName=this.selectedSubgroupName;

     if(this.groupallSelected) {
       cpy_selectedgroupName=[];
     }
     if(this.subgroupallSelected) {
       cpy_sselectedSubgroupName=[];
     }
     this.signalRService.connection
               .invoke('GetPivotData',this.SelectedDate,this.selectedTop,cpy_selectedgroupName.join(','),cpy_sselectedSubgroupName.join(','),this.selectedCKTName
               ,this.selected_Operator,this.valuemin,this.valuemax,1
               )
               .catch((error: any) => {
                 console.log(`GetPivotData error: ${error}`);
                 alert('GetPivotData error!, see console for details.');
           });
   }
}

