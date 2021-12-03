import React, { useState } from 'react';
import { Collapse, Container, Navbar, NavbarBrand, NavbarToggler, NavItem, NavLink, Button } from 'reactstrap';
import { Link, withRouter} from 'react-router-dom';
import { GoogleLogin } from 'react-google-login';
import './NavMenu.css';

const NavMenu = (props) => {
    const [toggleNavbar, setToggleNavbar] = useState(true);

    const responseGoogleSuccess = async (response) => {
        console.log(response);
        localStorage.setItem('email', response.getBasicProfile().getEmail());
        localStorage.setItem('googleId', response.getBasicProfile().getId());
        localStorage.setItem('accessToken', response.getAuthResponse(true).access_token);
        props.history.push('/signup');
    }

    const responseGoogleFailure = (response) => {
        console.log(response);
        props.history.push('/');
    }
    return (
        <header>
            <Navbar className="navbar-expand-sm navbar-toggleable-sm ng-white border-bottom box-shadow mb-3" light>
                <Container>
                    <NavbarBrand tag={Link} to="/">CarRental</NavbarBrand>
                    <NavbarToggler onClick={e => { setToggleNavbar(!toggleNavbar) }} className="mr-2" />
                    <Collapse className="d-sm-inline-flex flex-sm-row-reverse" isOpen={!toggleNavbar} navbar>
                        <ul className="navbar-nav flex-grow">
                            <NavItem className='nav-item'>
                                <NavLink tag={Link} className="text-dark" to="/" >Home</NavLink>
                            </NavItem>
                            <NavItem className='nav-item'>
                                <GoogleLogin
                                    clientId="626144450964-lgvp421untjh8h698e0pq5cvtpica9me.apps.googleusercontent.com"
                                    render={renderProps => (
                                        <Button onClick={renderProps.onClick} disabled={renderProps.disabled} color='primary'> Sign In </Button>
                                    )}
                                    buttonText="Login"
                                    onSuccess={responseGoogleSuccess}
                                    onFailure={responseGoogleFailure}
                                    cookiePolicy={'single_host_origin'}
                                />
                            </NavItem>
                        </ul>
                    </Collapse>
                </Container>
            </Navbar>
        </header>
    );
}

export default withRouter(NavMenu);
