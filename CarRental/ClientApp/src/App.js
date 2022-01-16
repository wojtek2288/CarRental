import React, { Component, Fragment } from 'react';
import { Route, Switch, Router } from 'react-router-dom';
import Home from './components/Home';

import './custom.css'

import UploadImage from './components/UploadImage';
import SignUp from './components/SignUp';
import AddCar from './components/AddCar';
import GroupedCars from './components/GroupedCarsPage';

import ProtectedRoute from './components/Routes/ProtectedRoute';
import UserHist from './components/UserHist';
import UserCurr from './components/UserCurr';
import AdminHist from './components/AdminHist';
import AdminCurr from './components/AdminCurr';

export default class App extends Component {
    static displayName = App.name;

    render() {
        return (
            <Switch>
                <Route exact path='/' component={Home} />
                <Route path='/signup' component={SignUp} />
                <Route path='/upload' component={UploadImage} />
                <ProtectedRoute path='/addcar' component={AddCar} role='Admin' />
                <ProtectedRoute path='/admin' component={GroupedCars} role='Admin' />
                <ProtectedRoute path='/rentalsadmin' component={AdminCurr} role='Admin' />
                <ProtectedRoute path='/archiveadmin' component={AdminHist} role='Admin' />
                <ProtectedRoute path='/user' component={GroupedCars} role='User' />
                <ProtectedRoute path='/rentalsuser' component={UserCurr} role='User' />
                <ProtectedRoute path='/archiveuser' component={UserHist} role='User' />
            </Switch>
        );
    }
}

