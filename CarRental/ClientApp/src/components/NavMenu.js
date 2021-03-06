import React, { Fragment, useState } from 'react';
import { Collapse, Container, Navbar, NavbarBrand, NavbarToggler, NavItem, NavLink, Button, DropdownToggle, DropdownItem, DropdownMenu, Dropdown } from 'reactstrap';
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
            localStorage.setItem('role', res.data);
        }
        catch (error) {
            console.log(error.response);

            //token jest niepoprawny (uzytkownik nigdy sie nie logowal) lub wygasl
            if (error.response.status === 401) {
                setUser({
                    loading: false
                });
                localStorage.removeItem('role');
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
                props.history.push('/user');
            else if (user.state === 'Admin')
                props.history.push('/admin');
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
        localStorage.removeItem('role');
        localStorage.removeItem('email');
        localStorage.removeItem('googleId');
        localStorage.removeItem('accessToken');
        localStorage.removeItem('tokenId');
        props.history.push('/');
    }

    const [dropdownOpen, setOpen] = useState(false);
    const toggle = () => setOpen(!dropdownOpen);

    return (
        <header>
            <Navbar className="navbar-expand-sm navbar-toggleable-sm ng-white border-bottom box-shadow mb-3" light>
                <Container>
                    <NavbarBrand>CarRental</NavbarBrand>
                        <ul className="navbar-nav flex-grow">
                            {props.button === 'hide' ? null : (
                                <Fragment>
                                    {props.logged === true ?
                                        (<Fragment>
                                            <GoogleLogout
                                                clientId="626144450964-lgvp421untjh8h698e0pq5cvtpica9me.apps.googleusercontent.com"
                                                render={renderProps => (
                                                    <Dropdown isOpen={dropdownOpen} toggle={toggle}>
                                                        <DropdownToggle tag="a" className="nav-link" type='button' caret>{localStorage.getItem('email')}</DropdownToggle>

                                                        {localStorage.getItem('role') === 'User' ?
                                                            (<DropdownMenu>
                                                                <DropdownItem tag={Link} to={"/user"}> Available Cars </DropdownItem>
                                                                <DropdownItem tag={Link} to={"/rentalsuser"}> Currently Rented Cars </DropdownItem>
                                                                <DropdownItem tag={Link} to={"/archiveuser"}> Rented Cars History </DropdownItem>
                                                                <DropdownItem divider />
                                                                <DropdownItem onClick={renderProps.onClick} disabled={renderProps.disabled} color='primary'> Sign Out </DropdownItem>
                                                            </DropdownMenu>)
                                                            :
                                                            (<DropdownMenu>
                                                                <DropdownItem tag={Link} to={"/admin"} >Available Cars</DropdownItem>
                                                                <DropdownItem tag={Link} to={"/rentalsadmin"}> Currently Rented Cars </DropdownItem>
                                                                <DropdownItem tag={Link} to={"/archiveadmin"}> Rented Cars History </DropdownItem>
                                                                <DropdownItem tag={Link} to={"/addcar"}>Add a Car</DropdownItem>
                                                                <DropdownItem divider />
                                                                <DropdownItem onClick={renderProps.onClick} disabled={renderProps.disabled} color='primary'>Sign Out</DropdownItem>
                                                            </DropdownMenu>)
                                                        }
                                                    </Dropdown>
                                                )}
                                                buttonText="Logout"
                                                onLogoutSuccess={logout}

                                            />
                                        </Fragment>)
                                        :
                                        (<GoogleLogin
                                            clientId="626144450964-lgvp421untjh8h698e0pq5cvtpica9me.apps.googleusercontent.com"
                                            render={renderProps => (
                                                <Button onClick={renderProps.onClick} disabled={renderProps.disabled} color='primary'> Sign In </Button>
                                            )}
                                            onSuccess={responseGoogleSuccess}
                                            onFailure={responseGoogleFailure}
                                            cookiePolicy={'single_host_origin'}
                                        />)
                                    }
                                </Fragment>
                            )}
                        </ul>
                </Container>
            </Navbar>
        </header>
    );
}

export default withRouter(NavMenu);
