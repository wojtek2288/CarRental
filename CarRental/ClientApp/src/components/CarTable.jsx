import React, { Fragment } from 'react';
import { useState } from 'react';
import { Button } from 'reactstrap';

export default function CarTable({ data }) {
    const [carId, setCarId] = useState(0)
    return (
        <table className='table table-striped' aria-labelledby="tabelLabel">
            <thead>
                <tr>
                    <th>Company</th>
                    <th>Brand</th>
                    <th>Model</th>
                    <th>Details</th>
                </tr>
            </thead>
            <tbody>
                {data.map(car =>
                    <tr key={car.id}>
                        <td>Car Rental</td>
                        <td>{car.brand}</td>
                        <td>{car.model}</td>
                        <td>
                            <Button onClick={() => setCarId(car.id)}
                                outline color="primary" type="button">See Details
                            </Button>
                            <div hidden={carId == car.id ? false : true}>
                                <b>Horse Power:</b>
                                <p>{car.horsepower}</p>
                                <b>Year of Production:</b>
                                <p>{car.yearOfProduction}</p>
                                <b>Description:</b>
                                <p>{car.description}</p>
                            </div>
                        </td>
                    </tr>
                )}
            </tbody>
        </table>)
}