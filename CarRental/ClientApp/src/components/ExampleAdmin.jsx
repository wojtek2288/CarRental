// Przykladowa sciezka dla admina
import React from 'react';
import { Fragment } from 'react';
import { Link, withRouter } from 'react-router-dom';
import NavMenu from './NavMenu';

const ExampleAdmin = (props) => {
    return (
        <Fragment>
            <NavMenu text='Sign Out' />
            <div>This is an example of page avaliable only for logged admins</div>
            <Link to='/addcar'>Add Car</Link>
        </Fragment>
    )
}

export default withRouter(ExampleAdmin);