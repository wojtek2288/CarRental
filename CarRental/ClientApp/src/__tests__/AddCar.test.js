import React from 'react';
import { shallow } from 'enzyme';
import AddCar from '../components/AddCar';
import axios from 'axios';
import { postCar } from '../Utils/utils';

jest.mock('axios');

describe("AddCar", () => {

    const car = {
        brand: 'Audi',
        model: 'A4',
        horsepower: 300,
        yearOfProduction: 2006,
        description: 'Cool audi'
    }
    test("Renders without crashing", () => {
        shallow(<AddCar/>);
    });

    test("Posting car", async () => {
        const spy = jest.spyOn(axios, 'post').mockImplementation(() => {
            return new Promise((resolve, reject) => {
                const result = {
                    status: 200,
                    data: '',
                };
                resolve(result);
            })
        });

        await postCar(car);

        expect(spy).toHaveBeenCalledTimes(1);
    });
});