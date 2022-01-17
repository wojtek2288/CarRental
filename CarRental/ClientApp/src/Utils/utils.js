import axios from 'axios';

export const postCar = async (formData) => {
    console.log(formData);
    try {
        const config = {
            headers: {
                'Content-Type': 'application/json'
            },
        }

        return await axios.post('/cars', formData, config);
    }
    catch (err) {
        console.log(err);
    }
}