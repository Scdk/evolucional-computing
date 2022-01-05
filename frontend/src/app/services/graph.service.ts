import { Injectable } from "@angular/core";

@Injectable()
export class GraphService{

  generateGraph(options: any, generations: Array<number>, bestFitness: Array<number>, avarageFitness: Array<number>){

    // for (let i = 0; i <= 1024; i++) {
    //   generations.push(i/2);
    //   bestFitness.push(419 + (- Math.abs(i/2 * Math.sin(Math.sqrt(Math.abs(i/2))))));
    // }

    options = {
      title: {
        text: 'Simple Exemple',
        subtext: 'Minimization of a function'
      },
      legend: {
        data: ['Target Function'],
        align: 'left',
      },
      tooltip: {},
      xAxis: {
        data: generations,
        silent: false,
        splitLine: {
          show: false,
        },
      },
      yAxis: {
      },
      dataZoom: [
        {
          show: true,
          type: 'inside',
          filterMode: 'none',
          xAxisIndex: [0],
          startValue: 0,
          endValue: 1024
        }
      ],
      series: [
        {
          name: 'Line',
          type: 'line',
          data: bestFitness,
          animationDelay: (idx: any) => idx * 10,
        },
        {
          name: 'Line',
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