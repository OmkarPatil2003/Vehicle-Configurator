/**
 * Purpose: Vehicle Configuration API service.
 * Handles fetching standard/available components (interior, exterior, accessories)
 * and managing local storage for ephemeral configuration state.
 */
import api from './api';

export const vehicleService = {
    // GET /vehicle/{modelId}/standard
    getStandardComponents: async (modelId) => {
        const response = await api.get(`/vehicle/${modelId}/standard`);
        return response.data;
    },

    // GET /vehicle/{modelId}/interior
    getInteriorComponents: async (modelId) => {
        const response = await api.get(`/vehicle/${modelId}/interior`);
        return response.data;
    },

    // GET /vehicle/{modelId}/exterior
    getExteriorComponents: async (modelId) => {
        const response = await api.get(`/vehicle/${modelId}/exterior`);
        return response.data;
    },

    // GET /vehicle/{modelId}/accessories
    getAccessoryComponents: async (modelId) => {
        const response = await api.get(`/vehicle/${modelId}/accessories`);
        return response.data;
    },

    // Helper to fetch all options at once
    getAllOptions: async (modelId) => {
        const [interior, exterior, accessories, standard] = await Promise.all([
            vehicleService.getInteriorComponents(modelId),
            vehicleService.getExteriorComponents(modelId),
            vehicleService.getAccessoryComponents(modelId),
            vehicleService.getStandardComponents(modelId)
        ]);

        return {
            Interior: interior,
            Exterior: exterior,
            Accessories: accessories,
            Standard: standard
        };
    },

    // POST /api/alternate-component/save
    // DTO: { modelId: Integer, components: [{ compId: Integer, altCompId: Integer }] }
    saveAlternateComponents: async (alternateComponentSaveDTO) => {
        const response = await api.post('/alternate-component/save', alternateComponentSaveDTO);
        return response.data;
    },

    // Local Storage Helpers
    saveSelection: (selection) => {
        sessionStorage.setItem('current_order_selection', JSON.stringify(selection)); // Changed to sessionStorage as per requirement
    },

    getSelection: () => {
        const data = sessionStorage.getItem('current_order_selection'); // Changed to sessionStorage as per requirement
        return data ? JSON.parse(data) : null;
    },

    clearSelection: () => {
        sessionStorage.removeItem('current_order_selection'); // Changed to sessionStorage as per requirement
        sessionStorage.removeItem('final_order'); // Changed to sessionStorage as per requirement
    }
};
