import React, {useState, useLayoutEffect} from 'react';
import { Route, Redirect } from 'react-router-dom';
import axios from 'axios';
import { Fragment } from 'react';
import { Spinner } from 'reactstrap';

const ProtectedRoute = ({ component: Component, role, ...rest }) => {
    const [user, setUser] = useState({
        state: '',
        loading: true
    });

    useLayoutEffect(() => {
        async function fetchData() {
            try {
                const config = {
                    headers: {
                        'Content-Type': 'application/json',
                        'AuthID': localStorage.getItem('googleId'),
                        'TokenID': localStorage.getItem('tokenId'),
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
                if (error.response.status === 401)
                {
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
        fetchData();
    }, []);

    return (
        <Fragment>
            {user.loading ?
                (<Spinner className='center'/>) :
                (<Route {...rest} render={() => user.state === role ? (< Component {...rest} />)
                    : (user.state === 'NotRegistered' ? (< Redirect to='/signup' />) : (<Redirect to='/'/>))} />)
            }
        </Fragment>
    )
}

export default ProtectedRoute;