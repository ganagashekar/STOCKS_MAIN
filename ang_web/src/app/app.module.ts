

import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpClientModule } from '@angular/common/http';
import { ListViewModule } from '@progress/kendo-angular-listview';
import { GridModule } from '@progress/kendo-angular-grid';
import { NavigationModule } from '@progress/kendo-angular-navigation';
import { IndicatorsModule } from '@progress/kendo-angular-indicators';
import { IconsModule } from '@progress/kendo-angular-icons';
import { LayoutModule } from '@progress/kendo-angular-layout';
import { ChartsModule } from '@progress/kendo-angular-charts';
import { ButtonsModule } from '@progress/kendo-angular-buttons';
import { TooltipsModule } from '@progress/kendo-angular-tooltip';
import 'hammerjs';
import { APP_INITIALIZER } from '@angular/core';
import { DayChartComponent } from './charts/day/day-chart.component';
import {MatSliderModule} from '@angular/material/slider';
import { AppComponent } from './app.component';
import { HeaderComponent } from './header/header.component';
import { TotalPortfolioComponent } from './total-portfolio/total-portfolio.component';
import { MyPortfolioComponent } from './my-portfolio/my-portfolio.component';
import { MainPanelComponent } from './main-panel/main-panel.component';
import { GridComponent } from './grid/grid.component';
import { BalanceComponent } from './main-panel/balance/balance.component';
import { TransactionsComponent } from './main-panel/transactions/transactions.component';
import { NewsComponent } from './main-panel/news/news.component';
import {SignalrService} from './services/signalr.service'

import { SliderColorDirective } from './directives/sildercolor';
// import { NgxSliderModule } from '@angular-slider/ngx-slider';
import { SliderComponeentComponent } from './charts/slider-Componeent/slider-Componeent.component';
import {ProgressBarMode, MatProgressBarModule} from '@angular/material/progress-bar';
import { WeekChartComponent } from './charts/week-chart/week-chart.component';
import { TopPerformerComponent } from './top-performer/top-performer.component';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import {MatDatepickerModule} from '@angular/material/datepicker';
import {MatInputModule} from '@angular/material/input';
import {MatFormFieldModule} from '@angular/material/form-field';
import {MatNativeDateModule} from '@angular/material/core';
import {MatSelectModule} from '@angular/material/select';
import {MatCardModule} from '@angular/material/card';
import {MatMenuModule} from '@angular/material/menu';
import {MatButtonModule} from '@angular/material/button';
import { PivotHistComponent } from './pivot-hist/pivot-hist.component';
import { CommonFiltersComponent } from './common-filters/common-filters.component';
import {MatAutocompleteModule} from '@angular/material/autocomplete';
import { FormControl, FormGroup,ReactiveFormsModule,FormsModule } from '@angular/forms';
import { MatToolbarModule } from '@angular/material/toolbar';
import {MatCheckboxModule} from '@angular/material/checkbox';
import {MatSidenavModule} from '@angular/material/sidenav';
import { MatIconModule } from '@angular/material/icon';
import { UpperlowerCKTComponent } from './upperlower-ckt/upperlower-ckt.component';
import { StockDaysComponent } from './stock-days/stock-days.component';
import { DynmaicResultsComponent } from './dynmaic-results/dynmaic-results.component';
import { StockdetailsComponent } from './stockdetails/stockdetails.component';
import { StocklistComponent } from './stocklist/stocklist.component';
import {MatChipsModule} from '@angular/material/chips';
import {MatBadgeModule} from '@angular/material/badge';
import { MAT_DATE_LOCALE } from '@angular/material/core'
import { FavoriteButtonComponent } from './favoriteButton/favoriteButton.component';
const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'top', component: TopPerformerComponent },
  { path: 'pivot', component: PivotHistComponent },
  { path: 'CKT', component: UpperlowerCKTComponent },
  { path: 'StockDAY', component: StockDaysComponent },
  { path: 'StockDetails', component: StockdetailsComponent },
  { path: 'Stocks', component: StocklistComponent },
  // { path: 'about', component: AboutComponent },
  // { path: 'products', component: ProductsComponent },
  // { path: '**', component: HomeComponent }, // If no matching route found, go back to home route
];

@NgModule({
    declarations: [	
        AppComponent,
        HeaderComponent,
        MyPortfolioComponent,
        MainPanelComponent,
        GridComponent,
        TotalPortfolioComponent,
        BalanceComponent,
        TransactionsComponent,
        NewsComponent,
        DayChartComponent,
        SliderComponeentComponent,
        WeekChartComponent,
        TopPerformerComponent,
        HomeComponent,
        PivotHistComponent,
        CommonFiltersComponent,
        UpperlowerCKTComponent,
        StockDaysComponent,
        DynmaicResultsComponent,
      StockdetailsComponent,
      StocklistComponent,
      FavoriteButtonComponent
   ],
    imports: [
      MatSliderModule,
      RouterModule.forRoot(routes),
      ListViewModule,
      MatSelectModule,
        BrowserModule,
        HttpClientModule,
        GridModule,
        BrowserAnimationsModule,
        NavigationModule,
        IndicatorsModule,
        IconsModule,
        LayoutModule,
        ButtonsModule,
        ChartsModule,
        TooltipsModule,
        MatProgressBarModule,
        MatDatepickerModule,
        MatInputModule,
        MatFormFieldModule,
        MatNativeDateModule,
        MatCardModule,
        MatButtonModule, MatMenuModule,MatAutocompleteModule,ReactiveFormsModule,FormsModule,MatCheckboxModule,MatSidenavModule,MatToolbarModule,MatIconModule,MatChipsModule,MatBadgeModule


    ],
    providers: [
      { provide: MAT_DATE_LOCALE, useValue: 'en-GB' },
      SignalrService,
    {
      provide: APP_INITIALIZER,
      useFactory: (signalrService: SignalrService) => () => signalrService.initiateSignalrConnection(),
      deps: [SignalrService],
      multi: true,
    },
    ],
    bootstrap: [AppComponent]
})
export class AppModule { }
