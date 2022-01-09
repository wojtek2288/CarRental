import React from 'react';
import RentedCars from './CarTableRented';

const UserRented = () => {
    return (<RentedCars url='/rentals/curr' role='User' title='Currently Rented Cars'/>);
}
export default UserRented;