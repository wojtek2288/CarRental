import React from 'react';
import RentedCars from './CarTableRented';

const UserRented = () => {
    return (<RentedCars url='/rentals/hist' role='User' title='Archived Bookings'/>);
    // return (<RentedCars url='/hist/Rented' role='User' title='Archived Bookings'/>);
}
export default UserRented;