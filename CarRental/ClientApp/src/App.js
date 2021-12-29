import React, { Component, Fragment } from 'react';
import { Route, Switch, Router } from 'react-router-dom';
import Home from './components/Home';

import './custom.css'
import SignUp from './components/SignUp';
import AddCar from './components/AddCar';
import ExampleUser from './components/ExampleUser';
import ExampleAdmin from './components/ExampleAdmin';
import ProtectedRoute from './components/Routes/ProtectedRoute';
import ViewCars from './components/ViewCars';

export default class App extends Component {
    static displayName = App.name;

    render() {
        return (
            <Switch>
                <Route exact path='/' component={Home} />
                <Route path='/signup' component={SignUp} />
                <ProtectedRoute path='/addcar' component={AddCar} role='Admin' />
                <ProtectedRoute path='/exampleadmin' component={ExampleAdmin} role='Admin' />
                <ProtectedRoute path='/exampleuser' component={ExampleUser} role='User' text='This is example prop' />
                <Route path='/viewcars' component={ViewCars} />
            </Switch>
        );
    }
}

