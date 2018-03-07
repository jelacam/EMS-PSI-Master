import React from 'react';
import dotnetify from 'dotnetify';
import { Line } from 'react-chartjs';

export default class LiveLineChart extends React.Component {
    constructor(props) {
        super(props);

        // Build the ChartJS data parameter with initial data.
        let initialData = {
            labels: [],
            datasets: [{
                label: "",
                data: [],
                fillColor: "rgba(217,237,245,0.2)",
                strokeColor: "#9acfea",
                pointColor: "#9acfea",
                pointStrokeColor: "#fff"
            }]
        };
        this.props.data.map(function (data) {
            initialData.labels.push(data[0]);
            initialData.datasets[0].data.push(data[1]);
        });
        this.state = { chartData: initialData };
    }
    render() {
        var chartData = this.state.chartData;
        const chartOptions = {
            responsive: true,
            animation: false,
            scaleShowGridLines: true,
            scaleGridLineColor: 'rgba(0,0,0,.05)',
            scaleGridLineWidth: 1,
            scaleShowHorizontalLines: true,
            scaleShowVerticalLines: true,
            bezierCurve: true,
            bezierCurveTension: 0.4,
            pointDot: true,
            pointDotRadius: 4,
            pointDotStrokeWidth: 1,
            pointHitDetectionRadius: 20,
            datasetStroke: true,
            datasetStrokeWidth: 2,
            datasetFill: true,
            legendTemplate: '<ul class=\"<%=name.toLowerCase()%>-legend\"><% for (var i=0; i<datasets.length; i++){%><li><span style=\"background-color:<%=datasets[i].strokeColor%>\"></span><%if(datasets[i].label){%><%=datasets[i].label%><%}%></li><%}%></ul>',
        };
        const updateData = data => {
            if (data) {
                chartData.labels.shift();
                chartData.labels.push(data[0]);
                chartData.datasets[0].data.shift();
                chartData.datasets[0].data.push(data[1]);
            }
        }
        return (
            <Line data={chartData} options={chartOptions}>{updateData(this.props.nextData)}></Line>
        );
    }
}