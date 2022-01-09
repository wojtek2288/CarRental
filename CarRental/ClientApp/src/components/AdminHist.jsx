import React from 'react';
import RentedCars from './CarTableRented';

const UserRented = () => {
    return (<RentedCars url='/rentals/hist' role='Admin' title='All Rented Cars'/>);
}
export default UserRented;