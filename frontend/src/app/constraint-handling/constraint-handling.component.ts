import { Component, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { ConstHandPost, ConstHandResponse } from '../models/contraint-handling';
import { ConstHandService } from '../services/contraint-handling.service';

@Component({
  selector: 'app-constraint-handling',
  templateUrl: './constraint-handling.component.html',
  providers: [ConstHandService]
})

export class ConstraintHandlingComponent implements OnInit {

  options: any;
	fittestIndividualVariables: number[] = [];
  fittestIndividualValue = 0;

  postForm = this.fb.group({
    populationSize: ['500'],
	  crossoverRate: ['0.6'],
    mutationRate: ['0.01'],
    elitismRate: ['0.1'],
    tournamentRate: ['0.01'],
    penaltyConstant: ['5000000'],
    numberOfVariables: ['2'],
    inferiorLimit: ['-3'],
    superiorLimit: ['5'],
	  numberOfGenerations: ['50'],
    challengeFlag: ['0']
  })

  constructor(private fb: FormBuilder, private constHandService: ConstHandService) { }

  ngOnInit(): void {
  }

  async callingFunction() {
    var postData: ConstHandPost = { 
      populationSize: this.postForm.value.populationSize,
      crossoverRate: this.postForm.value.crossoverRate,
      mutationRate: this.postForm.value.mutationRate,
      elitismRate: this.postForm.value.elitismRate,
      tournamentRate: this.postForm.value.tournamentRate,
      penaltyConstant: this.postForm.value.penaltyConstant,
      numberOfVariables: this.postForm.value.numberOfVariables,
      inferiorLimit: this.postForm.value.inferiorLimit,
      superiorLimit: this.postForm.value.superiorLimit,
      numberOfGenerations: this.postForm.value.numberOfGenerations,
      challengeFlag: this.postForm.value.challengeFlag
    }
    await this.constHandService.post(postData)
      .subscribe((data) => this.generateGraph(data));
  }

  generateGraph(data: any){
    var generations = data.map((x: ConstHandResponse) => 
      x.generation
    );
    var bestValue = data.map((x: ConstHandResponse) => 
      x.fittestIndividualValue
    );
    var avarageValue = data.map((x: ConstHandResponse) => 
      x.generationAvarageValue
    );
    this.fittestIndividualVariables = data[data.length - 1].fittestIndividualVariables;
    this.fittestIndividualValue = data[data.length - 1].fittestIndividualValue;

    this.options = {
      title: {
        text: 'Generation X Value',
        left: 'center'
      },
      legend: {
        data: ['Generations', 'Value'],
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
          name: 'Value',
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
          name: 'Best Value',
          type: 'line',
          data: bestValue,
          animationDelay: (idx: any) => idx * 10,
        },
        {
          name: 'Avarage Value',
          type: 'line',
          data: avarageValue,
          animationDelay: (idx: any) => idx * 10,
        }
      ],
      animationEasing: 'elasticOut',
      animationDelayUpdate: (idx: any) => idx * 5,
    };
  }
}
