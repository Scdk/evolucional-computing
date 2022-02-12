import { Component, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { DifEvoPost, DifEvoResponse } from '../models/differential-evolution';
import { DifEvoService } from '../services/differential-evolution.service';

@Component({
  selector: 'app-differential-evolution',
  templateUrl: './differential-evolution.component.html',
  providers: [DifEvoService]
})
export class DifferentialEvolutionComponent implements OnInit {

  options: any;
	fittestIndividualVariables: number[] = [];
  fittestIndividualValue = 0;

  postForm = this.fb.group({
    populationSize: ['100'],
	  crossoverRate: ['0.9'],
    numberOfVariables: ['2'],
    inferiorLimit: ['-1'],
    superiorLimit: ['2'],
    fValue: ['0.5'],
	  numberOfGenerations: ['50'],
    
  })

  constructor(private fb: FormBuilder, private difEvoService: DifEvoService) { }

  ngOnInit(): void {
  }

  async callingFunction() {
    var postData: DifEvoPost = { 
      populationSize: this.postForm.value.populationSize,
      crossoverRate: this.postForm.value.crossoverRate,
      numberOfVariables: this.postForm.value.numberOfVariables,
      inferiorLimit: this.postForm.value.inferiorLimit,
      superiorLimit: this.postForm.value.superiorLimit,
      fValue: this.postForm.value.fValue,
      numberOfGenerations: this.postForm.value.numberOfGenerations
    }
    await this.difEvoService.post(postData)
      .subscribe((data) => this.generateGraph(data));
  }

  generateGraph(data: any){
    var generations = data.map((x: DifEvoResponse) => 
      x.generation
    );
    var bestFitness = data.map((x: DifEvoResponse) => 
      x.fittestIndividualFitness
    );
    var avarageFitness = data.map((x: DifEvoResponse) => 
      x.generationAvarageFitness
    );
    this.fittestIndividualVariables = data[data.length - 1].fittestIndividualVariables;
    this.fittestIndividualValue = data[data.length - 1].fittestIndividualFitness;

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
          start: 0,
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
