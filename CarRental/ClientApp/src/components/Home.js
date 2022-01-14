import React, {useState, useEffect} from 'react';
import { Container, Button } from 'reactstrap';
import { GoogleLogin } from 'react-google-login';
import { Fragment } from 'react';
import axios from 'axios';
import NavMenu from './NavMenu';

const Home = (props) => {

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
                props.history.push('/');
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

    return (
        <Fragment>
            <NavMenu logged={false}/>
            <div className='home'>
                <Container>
                    <div className='home-text'>
                        <h1>Car Rental Service</h1>
                        <p>Welcome to Car Rental Service, where you can easily check and compare car rent prices in different companies.</p>
                        <GoogleLogin
                            clientId="626144450964-lgvp421untjh8h698e0pq5cvtpica9me.apps.googleusercontent.com"
                            render={renderProps => (
                                <Button onClick={renderProps.onClick} disabled={renderProps.disabled} className='btn-text-home' color='primary'> Sign In </Button>
                            )}
                            buttonText="Login"
                            onSuccess={responseGoogleSuccess}
                            onFailure={responseGoogleFailure}
                            cookiePolicy={'single_host_origin'}
                            isSignedIn={true}
                        />
                    </div>
                </Container>
            </div>
        </Fragment>
    );
}

export default Home;
