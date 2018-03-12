import React from 'react';
import dotnetify from 'dotnetify';
import { Line } from 'react-chartjs';
import Paper from 'material-ui/Paper';
import Divider from 'material-ui/Divider';
import { white, limeA400, amber800, limeA700, amber900, grey800 } from 'material-ui/styles/colors';


export default class LiveLineChart extends React.Component {
    constructor(props) {
        super(props);

        // Build the ChartJS data parameter with initial data.
        let initialData = {
            labels: [],
            datasets: [{
                label: "",
                data: [],
                fillColor: limeA400,
                strokeColor: limeA700,
                pointColor: 'rgb(153, 102, 255)',
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
            scaleGridLineWidth: 2,
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

        const styles = {
            paper: {
                backgroundColor: grey800,
                height: 220,
                width: 350,
                margin: 20,
                display: 'inline-block',
            },
            paper2: {
                backgroundColor: grey800,
                height: 220,
                width: 350,
                margin: 20,
                display: 'inline-block',
            },
            header: {
                fontSize: 24,
                color: white,
                backgroundColor: limeA700,
                padding: 10,
            },
            header2: {
                fontSize: 24,
                color: white,
                backgroundColor: amber900,
                padding: 10,
            },
            div: {
                height: 70,
                width: 300,
                padding: '5px 15px 5px 15px'
            }
        }

        return (
            <div>
                <div>
                    <Paper>
                        <div style={styles.header}>Generation: 1620kW</div>
                        <Paper style={styles.paper} zDepth={3} rounded={true} >
                            <div style={styles.header}>Grafik</div>
                            <div style={styles.div}>
                                <Line data={chartData} options={chartOptions}>{updateData(this.props.nextData)}></Line>
                            </div>
                        </Paper>
                        <Paper style={styles.paper} zDepth={3} rounded={true}>
                            <div style={styles.header}>Grafik</div>
                            <div style={styles.div}>
                                <Line data={chartData} options={chartOptions}>{updateData(this.props.nextData)}></Line>
                            </div>
                        </Paper>
                        <Paper style={styles.paper} zDepth={3} rounded={true}>
                            <div style={styles.header}>Grafik</div>
                            <div style={styles.div}>
                                <Line data={chartData} options={chartOptions}>{updateData(this.props.nextData)}></Line>
                            </div>
                        </Paper>
                        <Paper style={styles.paper} zDepth={3} rounded={true}>
                            <div style={styles.header}>Grafik</div>
                            <div style={styles.div}>
                                <Line data={chartData} options={chartOptions}>{updateData(this.props.nextData)}></Line>
                            </div>
                        </Paper>
                        <Paper style={styles.paper} zDepth={3} rounded={true}>
                            <div style={styles.header}>Grafik</div>
                            <div style={styles.div}>
                                <Line data={chartData} options={chartOptions}>{updateData(this.props.nextData)}></Line>
                            </div>
                        </Paper>
                        <Paper style={styles.paper} zDepth={3} rounded={true}>
                            <div style={styles.header}>Grafik</div>
                            <div style={styles.div}>
                                <Line data={chartData} options={chartOptions}>{updateData(this.props.nextData)}></Line>
                            </div>
                        </Paper>
                        <Paper style={styles.paper} zDepth={3} rounded={true}>
                            <div style={styles.header}>Grafik</div>
                            <div style={styles.div}>
                                <Line data={chartData} options={chartOptions}>{updateData(this.props.nextData)}></Line>
                            </div>
                        </Paper>
                        <Paper style={styles.paper} zDepth={3} rounded={true}>
                            <div style={styles.header}>Grafik</div>
                            <div style={styles.div}>
                                <Line data={chartData} options={chartOptions}>{updateData(this.props.nextData)}></Line>
                            </div>
                        </Paper>
                    </Paper>
                </div>
                <Divider />
                <div>
                    <Paper>
                        <div style={styles.header2}>Demand: 1619kW</div>
                        <Paper style={styles.paper2} zDepth={3} rounded={true}>
                            <div style={styles.header2}>Grafik</div>
                            <div style={styles.div}>
                                <Line data={chartData} options={chartOptions}>{updateData(this.props.nextData)}></Line>
                            </div>
                        </Paper>
                        <Paper style={styles.paper2} zDepth={3} rounded={true}>
                            <div style={styles.header2}>Grafik</div>
                            <div style={styles.div}>
                                <Line data={chartData} options={chartOptions}>{updateData(this.props.nextData)}></Line>
                            </div>
                        </Paper>
                        <Paper style={styles.paper2} zDepth={3} rounded={true}>
                            <div style={styles.header2}>Grafik</div>
                            <div style={styles.div}>
                                <Line data={chartData} options={chartOptions}>{updateData(this.props.nextData)}></Line>
                            </div>
                        </Paper>
                        <Paper style={styles.paper2} zDepth={3} rounded={true}>
                            <div style={styles.header2}>Grafik</div>
                            <div style={styles.div}>
                                <Line data={chartData} options={chartOptions}>{updateData(this.props.nextData)}></Line>
                            </div>
                        </Paper>
                        <Paper style={styles.paper2} zDepth={3} rounded={true}>
                            <div style={styles.header2}>Grafik</div>
                            <div style={styles.div}>
                                <Line data={chartData} options={chartOptions}>{updateData(this.props.nextData)}></Line>
                            </div>
                        </Paper>
                        <Paper style={styles.paper2} zDepth={3} rounded={true}>
                            <div style={styles.header2}>Grafik</div>
                            <div style={styles.div}>
                                <Line data={chartData} options={chartOptions}>{updateData(this.props.nextData)}></Line>
                            </div>
                        </Paper>
                        <Paper style={styles.paper2} zDepth={3} rounded={true}>
                            <div style={styles.header2}>Grafik</div>
                            <div style={styles.div}>
                                <Line data={chartData} options={chartOptions}>{updateData(this.props.nextData)}></Line>
                            </div>
                        </Paper>
                        <Paper style={styles.paper2} zDepth={3} rounded={true}>
                            <div style={styles.header2}>Grafik</div>
                            <div style={styles.div}>
                                <Line data={chartData} options={chartOptions}>{updateData(this.props.nextData)}></Line>
                            </div>
                        </Paper>
                    </Paper>
                </div>
            </div>
        );
    }
}