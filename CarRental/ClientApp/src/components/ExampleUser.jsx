// Przykladowa sciezka dla uzytkownika
import React from 'react';
import { Fragment } from 'react';
import NavMenu from './NavMenu';

const ExampleUser = (props) => {
    return (
        <Fragment>
            <NavMenu text='Sign Out' />
            <div>This is an example of page avaliable only for logged users {props.text}</div>
        </Fragment>
        )
}

export default ExampleUser;