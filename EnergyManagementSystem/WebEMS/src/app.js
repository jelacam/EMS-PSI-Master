import React from 'react';
import ReactDOM from 'react-dom';
import HelloWorld from 'HelloWorld.jsx';
import LineChart from 'LineChart.jsx';
import darkBaseTheme from 'material-ui/styles/baseThemes/darkBaseTheme';
import MuiThemeProvider from 'material-ui/styles/MuiThemeProvider';
import getMuiTheme from 'material-ui/styles/getMuiTheme';
import AppBar from 'material-ui/AppBar';

const data = [['a', 'b', 'c', 'd', 'e'], [65, 59, 80, 81, 56, 55, 40]];
let InitialLineData = [];

ReactDOM.render(
    <MuiThemeProvider muiTheme={getMuiTheme(darkBaseTheme)} >
        <LineChart InitialLineData={data} />
    </MuiThemeProvider>,
    document.getElementById('Content')
);