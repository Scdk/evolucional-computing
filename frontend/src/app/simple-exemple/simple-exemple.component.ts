import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-simple-exemple',
  templateUrl: './simple-exemple.component.html',
  styleUrls: ['./simple-exemple.component.css']
})
export class SimpleExempleComponent implements OnInit {

  options: any;

  constructor() { }

  ngOnInit(): void {

    const value = [];
    const targetFunctionResult = [];

    for (let i = 0; i <= 1024; i++) {
      value.push(i/2);
      targetFunctionResult.push(419 + (- Math.abs(i/2 * Math.sin(Math.sqrt(Math.abs(i/2))))));
    }

    this.options = {
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
        data: value,
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
          data: targetFunctionResult,
          animationDelay: (idx: any) => idx * 10,
        },
        {
          name: 'Points',
          type: 'scatter',
          data: [[421*2, 0.2891, 1]],
          symbol: 'circle',
          symbolSize: 10,
          color: 'black',
          itemStyle: {
            normal: {
              opacity: 1
            }
          }
        }
      ],
      animationEasing: 'elasticOut',
      animationDelayUpdate: (idx: any) => idx * 5,
    };
  }
}