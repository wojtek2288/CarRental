import React, { Fragment, useState, useEffect } from 'react';
import { Container } from 'reactstrap';
import NavMenu from './NavMenu';
import './CarTableStyles.css';

const Mixed = () => {
    const [data, setData] = useState([])
    
    useEffect(() => {
        fetch('/rentals/details')
            .then((response) => response.json())
            .then((json) => setData(json));
    }, [])

    console.log(data);

    const getDate = (hist) => {
        return new Date(hist.year, hist.month, hist.day);
    }

    return (
        <Fragment>
            <NavMenu logged={true}/>
            <Container className='margin-top'>
                <table id="car-list">
                    <thead>
                        <tr>
                            <th>Brand</th>
                            <th>Model</th>
                            <th>Rented Until</th>
                        </tr>
                    </thead>
                    <tbody>
                    {data.map(rented=>
                        <tr key={rented.id}>
                            <td>{rented.brand}</td>
                            <td>{rented.model}</td>
                            <td>{getDate(rented).toDateString()}</td>
                        </tr>
                    )}
                    </tbody>
                </table>
            </Container>
        </Fragment>
    );

}
export default Mixed;