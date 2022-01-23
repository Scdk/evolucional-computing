import { Component, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { ContParamsPost, ContParamsResponse } from '../models/continuous-parameters';
import { ContParamsService } from '../services/continuous-parameters.service';

@Component({
  selector: 'app-continuous-parameters',
  templateUrl: './continuous-parameters.component.html',
  providers: [ContParamsService]
})
export class ContinuousParametersComponent implements OnInit {

  options: any;
	fittestIndividualVariables: number[] = [];
  fittestIndividualValue = 0;

  postForm = this.fb.group({
    populationSize: ['50'],
	  crossoverRate: ['0.6'],
    mutationRate: ['0.01'],
    elitismRate: ['0.1'],
    tournamentRate: ['0.16'],
    numberOfVariables: ['2'],
    inferiorLimit: ['-5.12'],
    superiorLimit: ['5.12'],
	  numberOfGenerations: ['50'],
    
  })

  constructor(private fb: FormBuilder, private contParamsService: ContParamsService) { }

  ngOnInit(): void {
  }

  async callingFunction() {
    var postData: ContParamsPost = { 
      populationSize: this.postForm.value.populationSize,
      crossoverRate: this.postForm.value.crossoverRate,
      mutationRate: this.postForm.value.mutationRate,
      elitismRate: this.postForm.value.elitismRate,
      tournamentRate: this.postForm.value.tournamentRate,
      numberOfVariables: this.postForm.value.numberOfVariables,
      inferiorLimit: this.postForm.value.inferiorLimit,
      superiorLimit: this.postForm.value.superiorLimit,
      numberOfGenerations: this.postForm.value.numberOfGenerations
    }
    await this.contParamsService.post(postData)
      .subscribe((data) => this.generateGraph(data));
  }

  generateGraph(data: any){
    var generations = data.map((x: ContParamsResponse) => 
      x.generation
    );
    var bestFitness = data.map((x: ContParamsResponse) => 
      x.fittestIndividualFitness
    );
    var avarageFitness = data.map((x: ContParamsResponse) => 
      x.generationAvarageFitness
    );
    this.fittestIndividualVariables = data[data.length - 1].fittestIndividualVariables;
    this.fittestIndividualValue = 100 - data[data.length - 1].fittestIndividualFitness;

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
