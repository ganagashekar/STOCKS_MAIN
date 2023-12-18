import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { TopPerformerComponent } from '../top-performer/top-performer.component';


const routes: Routes = [
  { path: '', redirectTo: '/', pathMatch: 'full' },
  { path: 'top', component: TopPerformerComponent },
  // { path: 'about', component: AboutComponent },
  // { path: 'products', component: ProductsComponent },
  // { path: '**', component: HomeComponent }, // If no matching route found, go back to home route
];


@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    RouterModule.forRoot(routes)
  ]
})
export class AppRoutingModuleModule { }
