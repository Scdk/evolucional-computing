import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { MaximizationOfFunctionComponent } from './maximization-of-function/maximization-of-function.component';
import { MaximizationOfPotencyComponent } from './maximization-of-potency/maximization-of-potency.component';
import { SimpleExempleComponent } from './simple-exemple/simple-exemple.component';

const routes: Routes = [
  { path: '', redirectTo: '/maximization-of-potency', pathMatch: 'full' },
  { path: 'home', component: HomeComponent },
  { path: 'simple-exemple', component: SimpleExempleComponent},
  { path: 'maximization-of-function', component: MaximizationOfFunctionComponent},
  { path: 'maximization-of-potency', component: MaximizationOfPotencyComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
