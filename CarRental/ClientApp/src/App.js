import React, { Component, Fragment } from 'react';
import { Route, Switch, Router } from 'react-router-dom';
import Home from './components/Home';

import './custom.css'

import UploadImage from './components/UploadImage';
import SignUp from './components/SignUp';
import AddCar from './components/AddCar';
import GroupedCars from './components/GroupedCarsPage';

import ProtectedRoute from './components/Routes/ProtectedRoute';
import UserRented from './components/UserHist';
import UserRenting from './components/UserCurr';
import AllRented from './components/AdminHist';
import AllRenting from './components/AdminCurr';

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
                <ProtectedRoute path='/rentalsadmin' component={AllRenting} role='Admin' />
                <ProtectedRoute path='/archiveadmin' component={AllRented} role='Admin' />
                <ProtectedRoute path='/user' component={GroupedCars} role='User' />
                <ProtectedRoute path='/rentalsuser' component={UserRenting} role='User' />
                <ProtectedRoute path='/archiveuser' component={UserRented} role='User' />
            </Switch>
        );
    }
}

