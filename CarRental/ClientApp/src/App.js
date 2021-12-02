import React, { Component, Fragment } from 'react';
import { Route, Switch } from 'react-router-dom';
import Home from './components/Home';

import './custom.css'
import  NavMenu from './components/NavMenu';
import SignUp from './components/SignUp';
import AddCar from './components/AddCar';

export default class App extends Component {
    static displayName = App.name;

    render() {
        return (
            <Fragment>
                <NavMenu/>
                <Switch>
                    <Route exact path='/' component={Home} />
                    <Route path='/signup' component={SignUp} />
                    <Route path='/addcar' component={AddCar}/>
                </Switch>
            </Fragment>
        );
    }
}

