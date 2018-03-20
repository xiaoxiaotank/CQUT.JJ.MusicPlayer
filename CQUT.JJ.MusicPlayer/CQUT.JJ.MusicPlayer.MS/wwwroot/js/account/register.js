function afterRegister(data) {
    if (data.isSuccessed) {
        location.href = data.jsonObject.value;
    }
    else if (!data.isSuccessed) {
        fail_prompt(data.message);
    }
}