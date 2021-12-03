import React, { Component } from 'react';
import { Container, Button, Alert } from 'reactstrap';
import { Link } from 'react-router-dom';
import { GoogleLogin } from 'react-google-login';
import { useState } from 'react';


const Home = (props) => {
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
    );
}

export default Home;
