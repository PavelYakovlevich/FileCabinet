# FileCabinet

# Step5 - refactoring

| Class 1                    | Class 2            | Relationship |
|----------------------------|--------------------|--------------|
| FileCabinetService         | IRecordValidator   | has-a        |
| FileCabinetDefaultService  | FileCabinetService | is-a         |
| FileCabinetCustomService   | FileCabinetService | is-a         |
| FileCabinetDefaultService  | DefaultValidator   | has-a        |
| FileCabinetCustomService   | CustomValidator    | has-a        |
| DefaultValidator           | IRecordValidator   | is-a         |
| CustomValidator            | IRecordValidator   | is-a         |

# Step7 - add-filesystem

| Offset | Data Type | Field Size (bytes) | Name       | Description   |
|--------|-----------|--------------------|------------|---------------|
| 0      | short     | 2                  | Status     | Reserved      |
| 2      | int32     | 4                  | Id         | Record ID     |
| 6      | char[]    | 120                | FirstName  | First name    |
| 126    | char[]    | 120                | LastName   | Last name     |
| 246    | int32     | 4                  | Year       | Date of birth |
| 250    | int32     | 4                  | Month      | Date of birth |
| 254    | int32     | 4                  | Day        | Date of birth |
| 258    | int16     | 2                  | Stature    | Stature       |
| 260    | char      | 2                  | Gender     | Gender        |
| 262    | decimal   | 16                 | Weight     | Weight        |
