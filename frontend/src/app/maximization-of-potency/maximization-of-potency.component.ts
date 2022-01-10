import { Component, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { MaxPotPost, MaxPotResponse } from '../models/maximization-of-potency';
import { MaxPotService } from '../services/maximization-of-potency.service';

@Component({
  selector: 'app-maximization-of-potency',
  templateUrl: './maximization-of-potency.component.html',
  providers: [MaxPotService]
})
export class MaximizationOfPotencyComponent implements OnInit {

  options: any;
  scheduling: any;
	liquidPotencies: number[] = [];
  totalLiquidPotency: number = 0;
	fittestIndividualStandardDeviation: number = 0;

  postForm = this.fb.group({
    populationSize: [''],
	  crossoverRate: [''],
    mutationRate: [''],
	  numberOfGenerations: [''],
  })

  constructor(private fb: FormBuilder, private maxPotService: MaxPotService) { }

  ngOnInit(): void {
  }

  async callingFunction() {
    var postData: MaxPotPost = { 
      populationSize: this.postForm.value.populationSize,
	    crossoverRate: this.postForm.value.crossoverRate,
      mutationRate: this.postForm.value.mutationRate,
	    numberOfGenerations: this.postForm.value.numberOfGenerations
    }
    await this.maxPotService.post(postData)
      .subscribe((data) => this.generateGraph(data));
  }

  sumList(list: number[]): number{
    var sum: number = 0;
    list.forEach(element => {
      sum += element;
    });
    return sum;
  }

  generateGraph(data: any){
    var generations = data.map((x: MaxPotResponse) => 
      x.generation
    );
    var bestFitness = data.map((x: MaxPotResponse) => 
      x.fittestIndividualFitness
    );
    var avarageFitness = data.map((x: MaxPotResponse) => 
      x.generationAvarageFitness
    );
    this.scheduling = data[data.length - 1].scheduling;
    this.liquidPotencies = data[data.length - 1].liquidPotencies;
    this.totalLiquidPotency = this.sumList(this.liquidPotencies);
    this.fittestIndividualStandardDeviation = data[data.length - 1].fittestIndividualStandardDeviation;

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
