import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ConstraintHandlingComponent } from './constraint-handling/constraint-handling.component';
import { ContinuousParametersComponent } from './continuous-parameters/continuous-parameters.component';
import { HomeComponent } from './home/home.component';
import { MaximizationOfFunctionComponent } from './maximization-of-function/maximization-of-function.component';
import { MaximizationOfPotencyComponent } from './maximization-of-potency/maximization-of-potency.component';
import { SimpleExempleComponent } from './simple-exemple/simple-exemple.component';

const routes: Routes = [
  { path: '', redirectTo: '/constrait-handling', pathMatch: 'full' },
  { path: 'home', component: HomeComponent },
  { path: 'simple-exemple', component: SimpleExempleComponent},
  { path: 'maximization-of-function', component: MaximizationOfFunctionComponent},
  { path: 'maximization-of-potency', component: MaximizationOfPotencyComponent},
  { path: 'continuous-parameters', component: ContinuousParametersComponent},
  { path: 'constrait-handling', component: ConstraintHandlingComponent},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
