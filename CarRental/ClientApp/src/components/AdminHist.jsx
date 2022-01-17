import React from 'react';
import RentedCars from './CarTableRented';

const UserRented = () => {
    return (<RentedCars url='/rentals/hist' role='Admin' title='Rented Cars History'/>);
}
export default UserRented;