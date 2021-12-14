import React, { useState } from 'react';
import { Collapse, Container, Navbar, NavbarBrand, NavbarToggler, NavItem, NavLink, Button } from 'reactstrap';
import { Link, withRouter } from 'react-router-dom';
import { GoogleLogin, GoogleLogout } from 'react-google-login';
import axios from 'axios';
import './NavMenu.css';
import { useEffect } from 'react';

const NavMenu = (props) => {
    const [toggleNavbar, setToggleNavbar] = useState(true);

    const [user, setUser] = useState({
        state: '',
        loading: true
    });

    const fetchData = async () => {
        try {
            const config = {
                headers: {
                    'Content-Type': 'application/json',
                    'AuthID': localStorage.getItem('googleId'),
                    'TokenID': localStorage.getItem('tokenId')
                },
            }

            const res = await axios.get('/auth', config);
            console.log(res);

            //ustawiamy zwrocona role 
            setUser({
                state: res.data,
                loading: false
            });
        }
        catch (error) {
            console.log(error.response);

            //token jest niepoprawny (uzytkownik nigdy sie nie logowal) lub wygasl
            if (error.response.status === 401) {
                setUser({
                    loading: false
                });
                localStorage.removeItem('googleId');
                localStorage.removeItem('tokenId');
                localStorage.removeItem('accessToken');
                localStorage.removeItem('email');
            }
        }
    }

    useEffect(() => {
        if (user.loading === false) {
            if (user.state === 'User')
                props.history.push('/exampleuser');
            else if (user.state === 'Admin')
                props.history.push('/exampleadmin');
            else if (user.state === 'NotRegistered')
                props.history.push('/signup');
            else if (user.state === '')
                props.history.push('/');
        }
    }, [user])

    const responseGoogleSuccess = (response) => {
        console.log(response);
        localStorage.setItem('email', response.getBasicProfile().getEmail());
        localStorage.setItem('googleId', response.getBasicProfile().getId());
        localStorage.setItem('accessToken', response.getAuthResponse(true).access_token);
        localStorage.setItem('tokenId', response.getAuthResponse(true).id_token);

        fetchData();
    }

    const responseGoogleFailure = (response) => {
        console.log(response);
        props.history.push('/');
    }

    const logout = (response) => {
        console.log(response);
        localStorage.removeItem('email');
        localStorage.removeItem('googleId');
        localStorage.removeItem('accessToken');
        localStorage.removeItem('tokenId');
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
                            {props.button === 'hide' ? null : (
                                <NavItem className='nav-item'>
                                    {props.text === 'Sign Out' ?
                                        (<GoogleLogout
                                            clientId="626144450964-lgvp421untjh8h698e0pq5cvtpica9me.apps.googleusercontent.com"
                                            render={renderProps => (
                                                <Button onClick={renderProps.onClick} disabled={renderProps.disabled} color='primary'> {props.text} </Button>
                                            )}
                                            buttonText="Logout"
                                            onLogoutSuccess={logout}
                                        />)
                                        :
                                        (<GoogleLogin
                                            clientId="626144450964-lgvp421untjh8h698e0pq5cvtpica9me.apps.googleusercontent.com"
                                            render={renderProps => (
                                                <Button onClick={renderProps.onClick} disabled={renderProps.disabled} color='primary'> {props.text} </Button>
                                            )}
                                            onSuccess={responseGoogleSuccess}
                                            onFailure={responseGoogleFailure}
                                            cookiePolicy={'single_host_origin'}
                                        />)}

                                </NavItem>
                            )}
                        </ul>
                    </Collapse>
                </Container>
            </Navbar>
        </header>
    );
}

export default withRouter(NavMenu);
