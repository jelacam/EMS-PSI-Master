import React from 'react';
import dotnetify from 'dotnetify';
import LiveLineChart from 'LiveLineChart.jsx'

export default class LineChart extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            InitialLineData: props.InitialLineData
        }
        // Connect this component to the back-end view model.
        this.vm = dotnetify.react.connect("LiveLineData", this);
    }
    componentWillUnmount() {
        this.vm.$destroy();
    }
    render() {
        return (
            <LiveLineChart data={this.state.InitialLineData} nextData={this.state.NextLineData} />
        );
    }
}