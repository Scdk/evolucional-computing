import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { SimpleExempleComponent } from './simple-exemple/simple-exemple.component';
import { HomeComponent } from './home/home.component';
import { NgxEchartsModule } from 'ngx-echarts';
import { MaximizationOfFunctionComponent } from './maximization-of-function/maximization-of-function.component';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MaximizationOfPotencyComponent } from './maximization-of-potency/maximization-of-potency.component';
import { ContinuousParametersComponent } from './continuous-parameters/continuous-parameters.component';

@NgModule({
  declarations: [
    AppComponent,
    SimpleExempleComponent,
    HomeComponent,
    MaximizationOfFunctionComponent,
    MaximizationOfPotencyComponent,
    ContinuousParametersComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    NgxEchartsModule.forRoot({
      echarts: () => import('echarts')
    }),
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
