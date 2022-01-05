import { Component, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { MOFPost, MOFResponse } from '../models/maximization-of-function';
import { MOFService } from '../services/maximization-of-function.service';

@Component({
  selector: 'app-maximization-of-function',
  templateUrl: './maximization-of-function.component.html',
  providers: [MOFService]
})
export class MaximizationOfFunctionComponent implements OnInit {

  options: any;
  fitnestIndividualValueOfX : any;
  fitnestIndividualValueOfY : any;

  postForm = this.fb.group({
    populationSize: [''],
	  crossoverRate: [''],
    mutationRate: [''],
	  numberOfGenerations: [''],
  })

  constructor(private fb: FormBuilder, private mofService: MOFService) { }

  ngOnInit(): void {
  }

  async callingFunction() {
    var postData: MOFPost = { 
      populationSize: this.postForm.value.populationSize,
	    crossoverRate: this.postForm.value.crossoverRate,
      mutationRate: this.postForm.value.mutationRate,
	    numberOfGenerations: this.postForm.value.numberOfGenerations
    }
    var response;
    await this.mofService.post(postData)
      .subscribe((data) => this.generateGraph(data));
  }

  generateGraph(data: any){
    var generations = data.map((x: MOFResponse) => 
      x.generation
    );
    var bestFitness = data.map((x: MOFResponse) => 
      x.fittestIndividualFitness
    );
    var avarageFitness = data.map((x: MOFResponse) => 
      x.generationAvarageFitness
    );
    this.fitnestIndividualValueOfX = data[data.length - 1].fittestIndividualValueOfX;
    this.fitnestIndividualValueOfY = data[data.length - 1].fittestIndividualValueOfY;

    this.options = {
      title: {
        text: 'Generation X Fitness',
        left: 'center'
      },
      legend: {
        data: ['Generations', 'Fitness'],
        align: 'center',
      },
      tooltip: {},
      xAxis: {
        name: 'Generations',
        nameLocation: 'center',
        nameGap: 30,
        nameTextStyle: {
          fontWeight: 'bold',
          fontSize: 18,
          align: 'center',
          verticalAlign: 'top'
        },
        data: generations,
        type: "category",
        silent: false,
        splitLine: {
          show: false,
        },
      },
      yAxis: [
        {
          name: 'Fitness',
          nameLocation: 'center',
          nameGap: 30,
          nameTextStyle: {
            fontWeight: 'bold',
            fontSize: 18,
            align: 'center',
            verticalAlign: 'bottom'
          },
        }
      ],
      dataZoom: [
        {
          show: true,
          type: 'inside',
          filterMode: 'none',
          xAxisIndex: [0],
          start: 0,
          end: 100
        },
        {
          show: true,
          type: 'inside',
          filterMode: 'none',
          yAxisIndex: [0],
          start: 65,
          end: 100
        },
      ],
      series: [
        {
          name: 'Best Fitness',
          type: 'line',
          data: bestFitness,
          animationDelay: (idx: any) => idx * 10,
        },
        {
          name: 'Avarage Fitness',
          type: 'line',
          data: avarageFitness,
          animationDelay: (idx: any) => idx * 10,
        }
      ],
      animationEasing: 'elasticOut',
      animationDelayUpdate: (idx: any) => idx * 5,
    };
  }
}
