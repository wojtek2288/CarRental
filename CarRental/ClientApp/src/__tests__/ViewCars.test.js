import React from 'react';
import { shallow } from 'enzyme';
import ViewCars from '../components/ViewCars';

describe("ViewCars", () => {
    test("Renders without crashing", () => {
        shallow(<ViewCars />);
    });
});






