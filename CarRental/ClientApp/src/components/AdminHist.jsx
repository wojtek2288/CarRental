import React from 'react';
import RentedCars from './CarTableRented';

const UserRented = () => {
    return (<RentedCars url='/rentals/hist' role='Admin' title='Rented Cars Archive'/>);
    // return (<RentedCars url='/hist/Rented' role='Admin' title='Rented Cars Archive'/>);
}
export default UserRented;